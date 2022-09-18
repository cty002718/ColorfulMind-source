using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField]
    protected Transform trDest;                         //目的地

    [SerializeField]
    static protected Image imgBlack;                //黑色畫面
    static protected bool isTransporting = false;     //是否正在傳送
    static public bool IsTransporting { get { return isTransporting; } }

    static protected float fLerpSpeed = 1.25f;         //畫面變暗的速度
    static protected float fActiveDest = 1.5f;        //互動的最小範圍

    protected HeroController hc;

    [SerializeField]
    protected bool bCanPass = true;
    public bool BCanPass
    {
        get { return bCanPass; }
        set { bCanPass = value; }
    }

    [SerializeField] public string audioName = null;
    [SerializeField] protected float audioSpeed = 1;

    protected static CinemachineConfiner vcConfiner;

    protected void Awake()
    {
        if (!vcConfiner) { vcConfiner = GameObject.Find("CM vcam1").GetComponent<CinemachineConfiner>(); }
    }

    protected virtual void Start()
    {
        imgBlack = GameObject.Find("BlackScreen").GetComponent<Image>();
        imgBlack.enabled = false;
    }

    #region public function
    public void Transport(Transform _tr)
    {
        if (isTransporting) return;
        if (trDest == null)
        {
            Debug.Log("沒有設定傳送點");
            return;
        }
        //if (Vector2.Distance(_tr.position, transform.position) < fActiveDest)
        if (BCanPass)
        {
            hc = _tr.GetComponent<HeroController>();
            StartCoroutine(TransportTask(_tr));
        }
        else
        {
            GetComponent<NewDialogueTrigger>().TriggerDialogue();
        }
    }
    #endregion

    #region transport routine
    protected virtual IEnumerator TransportTask(Transform _tr)
    {
        isTransporting = true;
        imgBlack.enabled = true;
        if (audioName != null && audioName != "") { AudioController.instance.Mute(audioSpeed); }
        //畫面變暗
        float Proc = 0;
        while (imgBlack.color != Color.black) 
        {
            imgBlack.color = Color.Lerp(Color.clear, Color.black, Proc +=  fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        //Transport
        _tr.position = trDest.position;
        if(trDest.parent.Find("CameraConfiner") != null)
            vcConfiner.m_BoundingShape2D = trDest.parent.Find("CameraConfiner").GetComponent<PolygonCollider2D>();

        yield return new WaitForSeconds(0.2f);
        //畫面恢復
        Proc = 0;
        while (imgBlack.color != Color.clear)
        {
            imgBlack.color = Color.Lerp(Color.black, Color.clear, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        
        if (audioName != null && audioName != "") { 
            AudioController.instance.Open(audioSpeed);
            AudioController.instance.PlayBgm(audioName);
        }

        imgBlack.enabled = false;
        isTransporting = false;
    }
    #endregion
}
