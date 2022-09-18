using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class StatueDialogueTrigger : NewDialogueTrigger
{
    [SerializeField]
    protected Transform trDest;
    [SerializeField]
    protected Image imgBlack;
    [SerializeField]
    protected Camera camSource;
    [SerializeField]
    protected Camera camDest;
    [SerializeField]
    protected CinemachineConfiner vcConfiner;
    float fLerpSpeed = 1.25f;

    [SerializeField]
    string sBgm;

    public override void TriggerDialogue()
    {
        if (dialoguePath == "Level4/statue2")
        {
            base.TriggerDialogue();
            StartCoroutine(WaitForCallback());
        }
        else
        {
            base.TriggerDialogue();
        }
    }

    private IEnumerator WaitForCallback()
    {
        yield return new WaitUntil(() => { return RemptyTool.ES_MessageSystem.ES_MessageSystem.instance.IsDoingTextTask == false; });
        this.Callback();
    }

    public override void Callback()
    {
        StartCoroutine(Transport());
    }

    private IEnumerator Transport()
    {
        HeroController.instance.isGameStop = true;
        imgBlack.enabled = true;

        AudioController.instance.Mute();

        //畫面變暗
        float Proc = 0;
        while (imgBlack.color != Color.black)
        {
            imgBlack.color = Color.Lerp(Color.clear, Color.black, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        //Transport
        HeroController.instance.transform.position = trDest.position;
        vcConfiner.m_BoundingShape2D = trDest.parent.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
        camSource.enabled = false;
        camDest.enabled = true;

        yield return new WaitForSeconds(0.2f);
        //畫面恢復
        Proc = 0;
        while (imgBlack.color != Color.clear)
        {
            imgBlack.color = Color.Lerp(Color.black, Color.clear, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        AudioController.instance.PlayBgm(sBgm);
        AudioController.instance.Open();

        imgBlack.enabled = false;
        HeroController.instance.isGameStop = false;
    }
}

