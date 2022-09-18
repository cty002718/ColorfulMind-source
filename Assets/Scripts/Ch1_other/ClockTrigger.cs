using RemptyTool.ES_MessageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTrigger : NewDialogueTrigger
{
    [SerializeField] protected Image imgBlack;
    [SerializeField] protected Transform[] trs;
    Vector3[] v3Pos;

    private float fLerpSpeed = 1.25f;

    
    void Start()
    {
        v3Pos = new Vector3[trs.Length];
        for(int i = 0; i < trs.Length; i++) { v3Pos[i] = trs[i].position; }
    }

    public override void TriggerDialogue()
    {
        if (!QuestManager.instance.CheckIfComplete("Statue"))
        {
            base.TriggerDialogue();
            StartCoroutine(WaitForCallback());
        }
        else
        {
            this.dialoguePath = "Level1/Clock_2";
            base.TriggerDialogue();
        }
    }

    public override void Callback()
    {
        StartCoroutine(ResetStatues());
    }

    private IEnumerator WaitForCallback()
    {
        yield return new WaitUntil(() => { return ES_MessageSystem.instance.IsDoingTextTask == false; });
        this.Callback();
    }

    private IEnumerator ResetStatues()
    {
        HeroController.instance.isGameStop = true;
        if (imgBlack.enabled == false) { imgBlack.enabled = true; }
        //畫面變暗
        float Proc = 0;
        while (imgBlack.color != Color.black)
        {
            imgBlack.color = Color.Lerp(Color.clear, Color.black, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }
        //Transport
        for(int i = 0; i < trs.Length; i++)
        {
            trs[i].position = v3Pos[i];
        }

        yield return new WaitForSeconds(0.2f);
        //畫面恢復
        Proc = 0;
        while (imgBlack.color != Color.clear)
        {
            imgBlack.color = Color.Lerp(Color.black, Color.clear, Proc += fLerpSpeed * Time.deltaTime);
            yield return null;
        }

        imgBlack.enabled = false;
        HeroController.instance.isGameStop = false;
    }
}
