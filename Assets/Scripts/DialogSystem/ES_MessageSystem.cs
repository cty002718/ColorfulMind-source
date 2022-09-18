using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using Cinemachine;
using UnityEngine.EventSystems;

namespace RemptyTool.ES_MessageSystem
{
    /// <summary>The messageSystem is made by Rempty EmptyStudio. 
    /// UserFunction
    ///     SetText(string) -> Make the system to print or execute the commands.
    ///     Next()          -> If the system is WaitingForNext, then it will continue the remaining contents.
    ///     AddSpecialCharToFuncMap(string _str, Action _act)   -> You can add your customized special-characters into the function map.
    /// Parameters
    ///     IsCompleted     -> Is the input text parsing completely by the system.
    ///     text            -> The result, witch you can show on your interface as a dialog.
    ///     IsWaitingForNext-> Waiting for user input -> The Next() function.
    ///     textSpeed       -> Setting the updating period of text.
    /// </summary> 
    public class ES_MessageSystem : MonoBehaviour
    {
        public static ES_MessageSystem instance;

        public Animator animator;

        #region Original param
        public bool IsCompleted { get { return IsMsgCompleted; } }
        public string text { get { return msgText; } }
        public bool IsWaitingForNext { get { return IsWaitingForNextToGo; } }
        [SerializeField]
        private float defaultTextSpeed = 0.01f;
        private float textSpeed; //Updating period of text. The actual period may not less than deltaTime.

        private const char SPECIAL_CHAR_STAR = '[';
        private const char SPECIAL_CHAR_END = ']';
        private const char HTML_TAG_BEGIN = '<';
        private const char HTML_TAG_END = '>';
        private enum SpecialCharType { StartChar, CmdChar, EndChar, NormalChar, HtmlStartChar, HtmlEndCHar }
        private bool IsMsgCompleted = true;
        private bool IsOnSpecialChar = false;
        private bool IsOnHtmlChar = false;
        private bool IsWaitingForNextToGo = false;
        private bool IsOnCmdEvent = false;
        private string specialCmd = "";
        private string msgText;
        private char lastChar = ' ';
        private Dictionary<string, Action> specialCharFuncMap = new Dictionary<string, Action>();

        #endregion

        #region Modified param
        private List<string> textList = new List<string>();     //訊息（以行為單位）
        private int textIndex = 0;                              //第幾行
        private string sCurLine;                                //目前行
        private Coroutine curLineTask;                          //目前行的coroutine
        private bool isDoingTextTask = false;
        public bool IsDoingTextTask { get { return isDoingTextTask; } }

        [SerializeField]
        protected Text textMsgBox;                                  //訊息框
        [SerializeField]
        protected Text textNameBox;                                 //名字對話框
        [SerializeField]
        protected Image imgSpeaker;                                 //說話者頭像框
        [SerializeField]
        protected Image imgShow;                                    //要顯示在畫面中央的圖
        private int index;                                      //目前到第幾個字

        [SerializeField]
        protected BtnSetting btnSetting;
        private Stack<Tuple<int, int, bool>> stackChoices = new Stack<Tuple<int, int, bool>>();      //用來紀錄選擇處理到的部份
                                                                                                     //可有多層巢狀選項
        private List<Coroutine> lChoicesRoutines = new List<Coroutine>();                            //與上面的stack配合使用的list
        private bool isChoosed = false;                     //是否做出選擇
        private int iChoosed;                               //選中的id

        private AudioSource audiosource;

        [SerializeField]
        protected string sPicturePath;
        [SerializeField]
        protected string sMusicPath;

        [SerializeField]
        protected Font font;

        [SerializeField]
        protected Text textMainBox;
        [SerializeField]
        protected Transform trDialogueBox;                         //底圖
        [SerializeField]
        protected Text textAnotherBox;
        [SerializeField]
        protected Transform trAnotherBox;

        private bool skipMode = false;
        private bool isSkipped = false;
        private bool isEnding = false;

        #endregion

        protected string curTag = "";                     //html tag
        protected Transform empty = null;               //空物件（移動相機用
        [SerializeField]
        protected CinemachineVirtualCamera virtualcamera;
        [SerializeField]
        protected Transform emptyPrefab;
        [SerializeField]
        protected Transform origionFollow;              //原本相機跟著的（玩家）

        NewDialogueTrigger trigger = null;

        #region Game loop
        void Start()
        {
            //Register the Keywords Function.
            specialCharFuncMap.Add("w", () => StartCoroutine(CmdFun_w_Task()));
            specialCharFuncMap.Add("r", () => StartCoroutine(CmdFun_r_Task()));
            specialCharFuncMap.Add("l", () => StartCoroutine(CmdFun_l_Task()));
            specialCharFuncMap.Add("lr", () => StartCoroutine(CmdFun_lr_Task()));
            specialCharFuncMap.Add("Music", PlayMusic);
            specialCharFuncMap.Add("/Music", EndMusic);
            specialCharFuncMap.Add("Picture", ShowPicture);
            specialCharFuncMap.Add("/Picture", DisablePicture);
            specialCharFuncMap.Add("Item", GiveItem);
            specialCharFuncMap.Add("QuestComplete", QuestComplete);
            specialCharFuncMap.Add("Say", SetName);
            specialCharFuncMap.Add("Choose", () => StartCoroutine(Choose()));
            specialCharFuncMap.Add("Skip", Skip);
            specialCharFuncMap.Add("/Skip", DeSkip);
            specialCharFuncMap.Add("Speed", SetSpeed);
            specialCharFuncMap.Add("End", EndDialogue);

            specialCharFuncMap.Add("ItemList", ManageItemList);
            specialCharFuncMap.Add("Close_And_Wait", () => StartCoroutine(CloseAndWait()));
            specialCharFuncMap.Add("Close", Close);
            specialCharFuncMap.Add("Open", Open);
            specialCharFuncMap.Add("Wait", () => StartCoroutine(WaitForSec()));
            specialCharFuncMap.Add("Animation", PlayAnimation);
            specialCharFuncMap.Add("TriggerAnimation", TriggerAnimation);
            specialCharFuncMap.Add("Move", () => StartCoroutine(Move()));
            specialCharFuncMap.Add("Hide", Hide);

            if (!trDialogueBox)
            {
                Debug.LogError("Dialog box not set.");
            }
            trDialogueBox.gameObject.SetActive(false);
            if (trAnotherBox) trAnotherBox.gameObject.SetActive(false);
            if (textMsgBox) textMsgBox.font = font;
            if (textNameBox) textNameBox.font = font;

            textSpeed = defaultTextSpeed;
        }

        private void Awake()
        {
            instance = this;
            //audiosource = this.GetComponent<AudioSource>();
        }

        private void Update()
        {
            UpdateText();
        }

        #endregion

        #region Public Function
        public void AddSpecialCharToFuncMap(string _str, Action _act)
        {
            specialCharFuncMap.Add(_str, _act);
        }
        #endregion

        #region User Function
        //Begin whole task
        public void BeginTextTask(NewDialogueTrigger _trigger, TextAsset _text)
        {
            if (IsDoingTextTask) return;
            ReadTextDataFromAsset(_text);
            trigger = _trigger;
            if (textList.Count != 0)
            {
                //Open dialogue box, check objects
                trDialogueBox.gameObject.SetActive(true);
                if (animator) animator.SetBool("isOpen", true);
                textMsgBox = textMainBox;
                if (textMsgBox == null)
                {
                    Debug.LogError("UI text Component not assign.");
                }

                //Set states
                if (textNameBox) textNameBox.text = "";
                if (imgSpeaker) imgSpeaker.gameObject.SetActive(false);
                if (imgShow) imgShow.gameObject.SetActive(false);
                Initiate();
            }
        }

        public void BeginTextTask_2(NewDialogueTrigger _trigger, TextAsset _text)
        {
            if (IsDoingTextTask) return;
            ReadTextDataFromAsset(_text);
            trigger = _trigger;
            if (textList.Count != 0)
            {
                //Open dialogue box, check objects
                trAnotherBox.gameObject.SetActive(true);
                //trAnotherBox.GetComponent<RectTransform>()
                textMsgBox = textAnotherBox;
                Vector2 size = trAnotherBox.GetComponent<RectTransform>().sizeDelta;
                textAnotherBox.GetComponent<RectTransform>().sizeDelta = size * 0.9f;
                if (textMsgBox == null)
                {
                    Debug.LogError("UI text Component not assign.");
                }

                //Set states
                //if (textNameBox) textNameBox.text = "";
                //if (imgSpeaker) imgSpeaker.gameObject.SetActive(false);
                //if (imgShow) imgShow.gameObject.SetActive(false);
                Initiate();
            }
        }

        #endregion

        #region Keywords Function
        private IEnumerator CmdFun_l_Task()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator CmdFun_r_Task()
        {
            IsOnCmdEvent = true;
            msgText += '\n';
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator CmdFun_w_Task()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            //Debug.Log(msgText);
            msgText = "";   //Erase the messages.
            IsOnCmdEvent = false;
            yield return null;
        }
        private IEnumerator CmdFun_lr_Task()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            msgText += '\n';
            IsOnCmdEvent = false;
            yield return null;
        }
        private void PlayMusic()
        {
            string sFileName = GetExtendedCmd();
            //Debug.Log(sMusicPath + sFileName);
            //play music
            if (audiosource) {
                AudioClip audio = Resources.Load<AudioClip>(sMusicPath + sFileName);
                if (audio)
                {
                    audiosource.clip = audio;
                    audiosource.Play();
                }
            }
        }
        private void EndMusic()
        {
            if (audiosource)
            {
                audiosource.Stop();
                audiosource.clip = null;
            }
        }
        private void ShowPicture()
        {
            //name,pos.x,pos.y,size.x,size.y
            string[] sCmd = GetExtendedCmd().Split(',');

            //show picture
            Sprite s = Resources.Load<Sprite>(sPicturePath + sCmd[0]);
            if (s && imgShow)
            {
                imgShow.gameObject.SetActive(true);
                imgShow.sprite = s;
                imgShow.rectTransform.anchoredPosition = new Vector2(int.Parse(sCmd[1]), int.Parse(sCmd[2]));
                imgShow.rectTransform.sizeDelta = new Vector2(int.Parse(sCmd[3]), int.Parse(sCmd[4]));
            }
        }
        private void DisablePicture()
        {
            if (imgShow) imgShow.gameObject.SetActive(false);
        }
        private void Hide()
        {
            if (animator) animator.SetBool("Hide", true);
        }
        private void GiveItem()
        {
            //string[] sItemInfo = GetExtendedCmd().Split(',');

            //give item
            ItemWorld itemWorld = trigger.GetComponent<ItemWorld>();
            Inventory.instance.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }

        // [QuestComplete] quest1, quest2...;
        private void QuestComplete() {
            string[] quests = GetExtendedCmd().Split(',');
            foreach (string quest in quests) {
                QuestManager.instance.MarkQuestComplete(quest);
            }
        }

        //[Say] cmd 設定名字
        private void SetName()
        {
            string sName = GetExtendedCmd();

            //SetName
            if (textNameBox.text != sName)
            {
                textNameBox.text = sName;
                if (imgSpeaker)
                {
                    if (sName != "")
                    {
                        Sprite s = Resources.Load<Sprite>(sPicturePath + sName);
                        if (s) { imgSpeaker.gameObject.SetActive(true); imgSpeaker.sprite = s; }
                        else imgSpeaker.gameObject.SetActive(false);
                    }
                    else imgSpeaker.gameObject.SetActive(false);
                }
            }
        }
        //[Choose] cmd 觸發選擇模式
        private IEnumerator Choose()
        {
            IsOnCmdEvent = true;
            isChoosed = false;

            string[] sChoices = GetExtendedCmd().Split(',');
            Button[] btnChoices = SetChoices(sChoices);
            yield return new WaitUntil(() => isChoosed == true);
            //Debug.Log(iChoosed.ToString());

            EndChoose(btnChoices);
            isSkipped = false;
            IsOnCmdEvent = false;
            yield return null;
        }
        //[End] cmd, 結束整個對話
        private void EndDialogue()
        {
            if (isEnding) return;
            isEnding = true;
            IsOnCmdEvent = true;
            textList.Clear();
            textIndex = 0;
            //isDoingTextTask = false;
            
            foreach (Coroutine c in lChoicesRoutines)
            {
                if (c != null) StopCoroutine(c);
            }
            lChoicesRoutines.Clear();
            stackChoices.Clear();
            
            if (curLineTask != null) StopCoroutine(curLineTask);
            curLineTask = null;
            
            //Close Dialogue box
            textMsgBox.text = "";
            this.msgText = "";
            if (animator) animator.SetBool("isOpen", false);
            
            if (trigger != null)
            {
                Animator trigger_animator = trigger.gameObject.GetComponent<Animator>();
                if (trigger_animator != null)
                {
                    trigger_animator.SetFloat("Look Y", -1);
                    trigger_animator.SetFloat("Look X", 0);
                }
            }
            
            if (trAnotherBox) trAnotherBox.gameObject.SetActive(false);
            if (audiosource)
            {
                audiosource.Stop();
                audiosource.clip = null;
            }
            
            trigger = null;

            if (empty != null)
            {
                virtualcamera.Follow = origionFollow;
                GameObject.Destroy(empty.gameObject);
                empty = null;
            }

            if (animator) { Invoke("CloseInvoke", 0.25f); }
            else
            {
                trDialogueBox.gameObject.SetActive(false);
                IsOnCmdEvent = false;
                isDoingTextTask = false; 
                isEnding = false;
            }
        }

        //[ItemList]ItemType,amount; 改變物品清單的數量
        private void ManageItemList()
        {
            string[] tokens = GetExtendedCmd().Split(',');
            if (tokens[0] == "Petal")
            {
                List<Item> il = Inventory.instance.GetItemList();
                //Debug.Log(tokens[0] + ", " + tokens[1]);
                for (int i = 0; i < il.Count; i++)
                {
                    if (il[i].itemType == Item.ItemType.Petal)
                    {
                        //Debug.Log("do" + int.Parse(tokens[1]));
                        il[i].amount += int.Parse(tokens[1]);
                        if (il[i].amount == 0) il.RemoveAt(i);
                        break;
                    }
                }
            }
            UI_Inventory.instance.RefreshInventory();
        }

        //[Close_And_Wait]關閉對話框並等待點擊
        private IEnumerator CloseAndWait()
        {
            IsOnCmdEvent = true;
            IsWaitingForNextToGo = true;
            Animator a = textMsgBox.transform.parent.GetComponent<Animator>();
            if (a != null)
            {
                a.SetBool("isOpen", false);
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitUntil(() => IsWaitingForNextToGo == false);
            msgText = "";   //Erase the messages.
            if (a) { a.SetBool("isOpen", true); }
            IsOnCmdEvent = false;
            yield return null;
        }

        //[Close] 關閉對話框但繼續讀對話（指令）
        private void Close()
        {
            IsOnCmdEvent = true;
            Animator a = textMsgBox.transform.parent.GetComponent<Animator>();
            if (a != null)
            {
                a.SetBool("Hide", false);
                a.SetBool("isOpen", false);
            }
            IsOnCmdEvent = false;
        }
        //[Open] 打開對話框
        private void Open()
        {
            IsOnCmdEvent = true;
            Animator a = textMsgBox.transform.parent.GetComponent<Animator>();
            if (a != null)
            {
                a.SetBool("isOpen", true);
                a.SetBool("Hide", false);
            }
            msgText = "";
            IsOnCmdEvent = false;
        }
        //[Wait]x; 等x秒後繼續
        private IEnumerator WaitForSec()
        {
            IsOnCmdEvent = true;
            float sec = float.Parse(GetExtendedCmd());
            //Debug.Log(sec);
            yield return new WaitForSeconds(sec);
            IsOnCmdEvent = false;
            yield return null;
        }
        //[Animation]name,animation_name,Type,Name,Value,...;
        //Type = F SetFloot
        //Type = B Setbool
        private void PlayAnimation()
        {
            IsOnCmdEvent = true;
            string[] tokens = GetExtendedCmd().Split(',');
            if (tokens.Length < 2) return;
            GameObject go = GameObject.Find(tokens[0]);
            if (!go) return;
            Animator at = go.GetComponent<Animator>();
            if (!at) return;
            string msg = tokens[0] + ", " + tokens[1];
            for (int i = 4; i < tokens.Length; i += 3)
            {
                if (tokens[i - 2] == "F")
                {
                    float value = float.Parse(tokens[i]);
                    //Debug.Log(Time.deltaTime + ", value = " + value);
                    at.SetFloat(tokens[i - 1], value);
                }
                else if (tokens[i - 2] == "B")
                {
                    bool value = bool.Parse(tokens[i]);
                    at.SetBool(tokens[i - 1], value);
                }
                msg += ", " + tokens[i - 2] + ", " + tokens[i - 1] + ", " + tokens[i];
            }
            at.Play(tokens[1]);
            //Debug.Log(msg);
            IsOnCmdEvent = false;
        }

        private void TriggerAnimation() {
            IsOnCmdEvent = true;
            string[] tokens = GetExtendedCmd().Split(',');
            if (tokens.Length < 2) return;
            GameObject go = GameObject.Find(tokens[0]);
            if (!go) return;
            Animator at = go.GetComponent<Animator>();
            if (!at) return;

            at.SetTrigger(tokens[1]);
            IsOnCmdEvent = false;
        }

        //[Move]x,y,speed;水平移動x格，垂直移動y格，速度可以省略
        private IEnumerator Move()
        {
            string[] tokens = GetExtendedCmd().Split(',');
            if (tokens.Length < 2) yield break;
            if (this.empty == null)
            {
                FollowEmpty();
            }

            Vector2 source = empty.position;
            Vector2 target = new Vector2(empty.position.x + float.Parse(tokens[0]), empty.position.y + float.Parse(tokens[1]));
            float process = 0;
            float speed = 1;
            if (tokens.Length == 3) { speed = float.Parse(tokens[2]); }
            while ((Vector2)empty.position != target)
            {
                empty.position = Vector2.Lerp(source, target, process);
                process += Time.deltaTime * speed;
                yield return null;
            }
        }
        private void FollowEmpty()
        {
            empty = GameObject.Instantiate(emptyPrefab);
            empty.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
            virtualcamera.Follow = empty;
        }

        //[Skip]
        private void Skip()
        {
            skipMode = true;
        }
        //[/Skip]
        private void DeSkip()
        {
            skipMode = false;
        }
        //[Speed]0.xx;
        private void SetSpeed()
        {
            this.textSpeed = float.Parse(GetExtendedCmd());
        }

        #endregion

        #region Messages Core
        private void Initiate()
        {
            textMsgBox.text = "";
            isDoingTextTask = true;
            IsMsgCompleted = true;
            IsOnSpecialChar = false;
            IsOnHtmlChar = false;
            IsWaitingForNextToGo = false;
            IsOnCmdEvent = false;
            specialCmd = "";
            textSpeed = defaultTextSpeed;
            isSkipped = false;
            skipMode = false;

            if (animator) animator.SetBool("Hide", false);

            empty = null;
            origionFollow = virtualcamera.Follow;
        }

        private void AddChar(char _char)
        {
            if (curTag != "")
            {
                msgText = msgText.Insert(msgText.LastIndexOf("<" + curTag + ">"), _char.ToString());
            }
            else
            {
                msgText += _char;
            }
            lastChar = _char;
        }
        private SpecialCharType CheckSpecialChar(char _char)
        {
            if (_char == SPECIAL_CHAR_STAR)
            {
                if (lastChar == SPECIAL_CHAR_STAR)
                {
                    specialCmd = "";
                    IsOnSpecialChar = false;
                    return SpecialCharType.NormalChar;
                }
                IsOnSpecialChar = true;
                return SpecialCharType.CmdChar;
            }
            else if (_char == SPECIAL_CHAR_END && IsOnSpecialChar)
            {
                //exe cmd!
                if (specialCharFuncMap.ContainsKey(specialCmd))
                {
                    specialCharFuncMap[specialCmd]();
                    //Debug.Log("The keyword : [" + specialCmd + "] execute!");
                }
                else
                    Debug.LogError("The keyword : [" + specialCmd + "] is not exist!");
                specialCmd = "";
                IsOnSpecialChar = false;
                return SpecialCharType.EndChar;
            }
            else if (_char == HTML_TAG_BEGIN && !IsOnSpecialChar)
            {
                specialCmd = "";
                IsOnHtmlChar = true;
                return SpecialCharType.HtmlStartChar;
            }
            else if (_char == HTML_TAG_BEGIN && IsOnSpecialChar && lastChar == '[')
            {
                specialCmd = "";
                IsOnSpecialChar = false;
                return SpecialCharType.NormalChar;
            }
            else if (_char == HTML_TAG_END && IsOnHtmlChar)
            {
                //Debug.Log(specialCmd);
                if (specialCmd == curTag) { curTag = ""; }
                else
                {
                    if (specialCmd.IndexOf('=') != -1)
                    {
                        curTag = "/" + specialCmd.Substring(0, specialCmd.IndexOf('='));
                    }
                    else
                    {
                        curTag = "/" + specialCmd;
                    }
                    msgText += ("<" + specialCmd + "><" + curTag + ">");
                }
                IsOnHtmlChar = false;
                specialCmd = "";
                return SpecialCharType.HtmlEndCHar;
            }
            else if (IsOnSpecialChar || IsOnHtmlChar)
            {
                specialCmd += _char;
                return SpecialCharType.CmdChar;
            }
            return SpecialCharType.NormalChar;
        }
        //Show line routine
        private IEnumerator SetTextTask()
        {
            IsOnSpecialChar = false;
            IsMsgCompleted = false;
            specialCmd = "";
            for (index = 0; index < sCurLine.Length; index++)
            {
                switch (CheckSpecialChar(sCurLine[index]))
                {
                    case SpecialCharType.NormalChar:
                        AddChar(sCurLine[index]);
                        lastChar = sCurLine[index];
                        if (!skipMode && !isSkipped) yield return new WaitForSeconds(textSpeed);
                        break;
                }
                lastChar = sCurLine[index];
                if (IsOnCmdEvent) yield return new WaitUntil(() => IsOnCmdEvent == false);
            }
            IsMsgCompleted = true;
            curLineTask = null;
            yield return null;
        }

        #region update functions
        //Equal to update
        private void UpdateText()
        {
            if (IsDoingTextTask)
            {
                if (Input.GetKeyDown(KeyCode.Space) && IsWaitingForNextToGo)
                {
                    //Continue the messages, stoping by [w] or [lr] keywords.
                    this.Next();
                }
                else if (Input.GetKeyDown(KeyCode.Space) && !IsWaitingForNextToGo && !IsCompleted)
                {
                    isSkipped = true;
                }

                //If the message is complete, stop updating text.
                if (this.IsCompleted == false)
                {
                    textMsgBox.text = this.text;
                }

                //Auto update from textList.
                if (this.IsCompleted == true && textList.Count > 0 && textIndex < textList.Count)
                {
                    //Debug.Log(stackChoices.Count);
                    if (stackChoices.Count() > 0 && lChoicesRoutines.Count() > 0 && lChoicesRoutines[lChoicesRoutines.Count() - 1] == null)
                    {
                        int temp = lChoicesRoutines.Count() - 1;
                        lChoicesRoutines[temp] = StartCoroutine(ExecuteChooseMode());
                    }
                    else if (stackChoices.Count > 0 && lChoicesRoutines.Count() > 0 && lChoicesRoutines[lChoicesRoutines.Count - 1] != null)
                    {

                    }
                    else
                    {
                        this.SetText(textList[textIndex]);
                        textIndex++;
                    }
                }
                else if (this.IsCompleted == true && textIndex >= textList.Count)
                {
                    EndDialogue();
                }
            }
        }
        //[l],[lr],[w]
        private void Next()
        {
            IsWaitingForNextToGo = false;
            isSkipped = false;
        }
        //Start a line
        private void SetText(string _text)
        {;
            sCurLine = _text;
            curLineTask = StartCoroutine(SetTextTask());
        }

        #endregion

        //Load given text
        private void ReadTextDataFromAsset(TextAsset _textAsset)
        {
            textList.Clear();
            textList = new List<string>();
            textIndex = 0;
            var lineTextData = _textAsset.text.Split('\n');
            //Debug.Log(lineTextData.Count<string>());
            foreach (string line in lineTextData)
            {
                textList.Add(line);
            }
        }
        //Get extended cmd after ']' and ended with ';'
        private string GetExtendedCmd()
        {
            index++;
            string sExtString = "";
            while (sCurLine[index] != ';')
            {
                sExtString += sCurLine[index];
                if (index >= sCurLine.Length - 1) break;
                index++;
            }
            return sExtString;
        }


        #region Core functions about choices
        private Button[] SetChoices(string[] sChoices)
        {
            Button[] btnChoices = new Button[sChoices.Length];
            for (int i = 0; i < btnChoices.Length; i++)
            {
                btnChoices[i] = Instantiate(btnSetting.btnPrefab, trDialogueBox).GetComponent<Button>();
                btnChoices[i].name = ((stackChoices.Count * 10) + i + 1).ToString();
                btnChoices[i].transform.GetChild(0).GetComponent<Text>().text = sChoices[i];
                btnChoices[i].transform.GetChild(0).GetComponent<Text>().color = btnSetting.textColor;
                btnChoices[i].transform.GetChild(0).GetComponent<Text>().font = font;
                btnChoices[i].transform.GetChild(0).GetComponent<Text>().fontSize = btnSetting.iBtnFontSize;
                btnChoices[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(btnSetting.v2BtnSize.x / 2 + btnSetting.v2BtnOffset.x, (btnChoices.Length - i) * (btnSetting.v2BtnSize.y + btnSetting.v2BtnOffset.y));
                btnChoices[i].GetComponent<RectTransform>().sizeDelta = btnSetting.v2BtnSize;
                btnChoices[i].image.color = btnSetting.color;
                ColorBlock cb = btnChoices[i].colors;
                cb.selectedColor = btnSetting.selectedColor;
                btnChoices[i].colors = cb;
                Button b = btnChoices[i];
                btnChoices[i].onClick.AddListener(() => { BtnCallback(b); });
            }
            btnChoices[0].Select();
            return btnChoices;
        }
        private void EndChoose(Button[] btnChoices)
        {
            stackChoices.Push(new Tuple<int, int, bool>(iChoosed, btnChoices.Length, false));
            Coroutine c = null;
            lChoicesRoutines.Add(c);
            foreach (Button b in btnChoices)
            {
                GameObject.Destroy(b.gameObject);
            }
            msgText = "";
        }
        private void BtnCallback(Button btn)
        {
            iChoosed = int.Parse(btn.name);
            isChoosed = true;
        }

        private IEnumerator ExecuteChooseMode()
        {
            int layer = stackChoices.Count;
            int target = stackChoices.Peek().Item1;
            int total = stackChoices.Peek().Item2 + (layer - 1) * 10;
            //Debug.Log(layer.ToString() + ", " + target + ", " + total + ", " + "[" + target.ToString() + "]");
            if (stackChoices.Peek().Item3 == false)
            {
                while (!textList[textIndex].Contains("[" + target.ToString() + "]") && textIndex < textList.Count)
                {
                    textIndex++;
                }
            }
            while (textIndex < textList.Count)
            {
                if (textList[textIndex].Contains("[" + target.ToString() + "]"))
                {
                    string sAlt = textList[textIndex].Substring(textList[textIndex].IndexOf("[" + target.ToString() + "]") + (2 + target.ToString().Length));
                    if (sAlt != string.Empty)
                    {
                        if (sAlt.Substring(0, 5) == "[End]") { sAlt = "\r";  }
                        this.SetText(sAlt);
                        //Debug.Log("Msg:" + sAlt + "  " + Time.time);
                        yield return new WaitUntil(() => curLineTask == null);
                        //Debug.Log("Return : " + Time.time);
                    }
                }
                else if (textList[textIndex].Contains("[/" + target.ToString() + "]"))
                {
                    string sAlt = textList[textIndex].Substring(0, textList[textIndex].IndexOf("[/" + target.ToString() + "]"));
                    if (sAlt != string.Empty)
                    {
                        this.SetText(sAlt);
                        yield return new WaitUntil(() => curLineTask == null);
                    }
                    break;
                }
                else if (textList[textIndex].Contains("[Choose]"))
                {
                    //Debug.Log("do");
                    this.SetText(textList[textIndex]);
                    yield return new WaitUntil(() => curLineTask == null);
                    yield return new WaitUntil(() => layer == lChoicesRoutines.Count);
                    //Debug.Log("do2");
                    continue;
                }
                else
                {
                    this.SetText(textList[textIndex]);
                    yield return new WaitUntil(() => curLineTask == null);
                }

                textIndex++;
            }
            //Debug.Log("Pop :" + Time.time);
            stackChoices.Pop();
            stackChoices.Push(new Tuple<int, int, bool>(target, total, true));


            if (stackChoices.Peek().Item3 == true && target != total && textIndex < textList.Count)
            {
                while (!textList[textIndex].Contains("[/" + total.ToString() + "]"))
                {
                    textIndex++;
                }
                if (textIndex < textList.Count)
                {
                    textIndex++;
                }
            }
            else if (stackChoices.Peek().Item3 == true && target == total && textIndex < textList.Count)
            {
                textIndex++;
            }
            lChoicesRoutines[lChoicesRoutines.Count - 1] = null;
            lChoicesRoutines.RemoveAt(lChoicesRoutines.Count - 1);
            stackChoices.Pop();
            //Debug.Log("I am layer " + layer + ", mewwage: " + textList[textIndex]);
            yield return null;
        }

        #endregion

        #endregion

        private void CloseInvoke()  
        {
            trDialogueBox.gameObject.SetActive(false);
            IsOnCmdEvent = false;
            isDoingTextTask = false; 
            isEnding = false; 
        }
    }

    [Serializable]
    public struct BtnSetting
    {
        public GameObject btnPrefab;        //按鈕
        public Vector2 v2BtnSize;           //大小
        public Vector2 v2BtnOffset;         //x的位置和y的間隔
        public int iBtnFontSize;            //字體大小
        public Color textColor;
        public Color color;
        public Color selectedColor;
    }
}
