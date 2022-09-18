using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ColorUI : MonoBehaviour
{
    [SerializeField]
    protected Vector2 v2BtnSize = new Vector2(30, 30);
    [SerializeField]
    protected Vector2 v2BtnOffset = new Vector2(0, 5);
    [SerializeField]
    protected GameObject btnPrefab;

    private int index = 1;

    protected bool isActive = false;
    protected bool isPushing = false;

    private void Update()
    {
        int len = transform.childCount;
        if (RemptyTool.ES_MessageSystem.ES_MessageSystem.instance.IsDoingTextTask || HeroController.instance.inventory_open)
        {
            this.GetComponent<Button>().enabled = false;
            this.GetComponent<Button>().image.enabled = false;
            for (int i = 0; i < len; i++)
            {
                Button child = transform.GetChild(i).GetComponent<Button>();
                child.enabled = false;
                child.image.enabled = false;
            }
        }
        else
        {
            this.GetComponent<Button>().enabled = true;
            this.GetComponent<Button>().image.enabled = true;
            for (int i = 0; i < len; i++)
            {
                Button child = transform.GetChild(i).GetComponent<Button>();
                child.enabled = true;
                child.image.enabled = true;
            }
        }
    }


    public void ShowChoices()
    {
        StopAllCoroutines();
        if (!isActive)
        {
            int len = transform.childCount;
            for (int i = len - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
            isActive = true;
            //創立按鈕
            CreateBtns();
            //讓按鈕跑出來
            //Coroutines
            PopBtns();
        }
        else
        {
            DisShow();
        }
    }

    private void DisShow()
    {
        //讓按鈕跑回來
        //Coroutines
        PushBtns();
    }

    private void CreateBtns()
    {
        Button btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
        btn.image.color = new Color(1, 1, 1, 0.8f);
        Button b1 = btn;
        btn.transform.SetParent(transform);
        btn.onClick.AddListener( () =>  ChooseWhite(b1));
        btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
        btn.image.color = new Color(0, 0, 0, 0.8f);
        Button b2 = btn;
        btn.transform.SetParent(transform);
        btn.onClick.AddListener(() => ChooseBlack(b2));
        if (ColorSystem.ColorProfile.instance.IsBlueUnlocked)
        {
            btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
            btn.image.color = new Color(0.3f, 0.5f, 1, 0.8f);
            Button b3 = btn;
            btn.transform.SetParent(transform);
            btn.onClick.AddListener(() => ChooseBlue(b3));
        }
        if (ColorSystem.ColorProfile.instance.IsGreenUnlocked)
        {
            btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
            btn.image.color = new Color(0.6f, 1f, 0.6f, 0.8f);
            Button b4 = btn;
            btn.transform.SetParent(transform);
            btn.onClick.AddListener(() => ChooseGreen(b4));
        }
        if (ColorSystem.ColorProfile.instance.IsYellowUnlocked)
        {
            btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
            btn.image.color = new Color(1f, 1f, 0.4f, 0.8f);
            Button b5 = btn;
            btn.transform.SetParent(transform);
            btn.onClick.AddListener(() => ChooseYellow(b5));
        }
        if (ColorSystem.ColorProfile.instance.IsOrangeUnlocked)
        {
            btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
            btn.image.color = new Color(1f, 0.56f, 0, 0.8f);
            Button b6 = btn;
            btn.transform.SetParent(transform);
            btn.onClick.AddListener(() => ChooseOrange(b6));
        }
        if (ColorSystem.ColorProfile.instance.IsRedUnlocked)
        {
            btn = Instantiate<GameObject>(btnPrefab).GetComponent<Button>();
            btn.image.color = new Color(1f, 0.3f, 0.3f, 0.8f);
            Button b7 = btn;
            btn.transform.SetParent(transform);
            btn.onClick.AddListener(() => ChooseRed(b7));
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Button b = transform.GetChild(i).GetComponent<Button>();
            b.GetComponent<RectTransform>().sizeDelta = v2BtnSize;
            b.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            b.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            b.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            b.GetComponent<RectTransform>().localScale = Vector3.one;
            b.gameObject.name = index.ToString();
            index++;
        }
    }

    private void PopBtns()
    {
        int len = transform.childCount;
        for (int i = 0; i < len; i++)
        {
            float y = 25 + v2BtnSize.y * i + v2BtnOffset.y * (i + 1) + v2BtnSize.y / 2;
            Transform child = transform.GetChild(i);
            child.GetComponent<Button>().enabled = false;
            StartCoroutine(PopBtn(child.GetComponent<Button>(), y));
        }
    }
    private IEnumerator PopBtn(Button btn, float y)
    {
        RectTransform rt = btn.GetComponent<RectTransform>();
        float process = 0;
        while (rt.anchoredPosition.y != y)
        {
            rt.anchoredPosition = new Vector2(0, Mathf.Clamp(Mathf.Lerp(0, y, process), rt.anchoredPosition.y, rt.anchoredPosition.y + 5));
            process += Time.deltaTime * 9f;
            yield return null;
        }
        btn.enabled = true;
    }

    private void PushBtns()
    {
        int len = transform.childCount;
        for (int i = 0; i < len; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetComponent<Button>().enabled = false;
            StartCoroutine(PushBtn(child.GetComponent<Button>()));
        }
        isActive = false;
    }
    private IEnumerator PushBtn(Button bt)
    {
        float process = 0;
        RectTransform rt = bt.GetComponent<RectTransform>();
        float y = rt.anchoredPosition.y;
        bt.enabled = false;
        while (rt.anchoredPosition.y != 0)
        {
            rt.anchoredPosition = new Vector2(0, Mathf.Clamp(Mathf.Lerp(y, 0, process), rt.anchoredPosition.y - 5, rt.anchoredPosition.y));
            process += Time.deltaTime * 9f;
            yield return null;
        }
        GameObject.Destroy(bt.gameObject);
    }


    public void ChooseWhite(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.White);
        DisShow();
    }
    public void ChooseBlack(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.Black);
        DisShow();
    }
    public void ChooseBlue(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.Blue);
        DisShow();
    }
    public void ChooseGreen(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.Green);
        DisShow();
    }
    public void ChooseYellow(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.Yellow);
        DisShow();
    }
    public void ChooseOrange(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.Orange);
        DisShow();
    }
    public void ChooseRed(Button callback)
    {
        GetComponent<Button>().image.color = callback.image.color;
        HeroController.instance.SelectColor(ColorSystem.ColorType.Red);
        DisShow();
    }
}
