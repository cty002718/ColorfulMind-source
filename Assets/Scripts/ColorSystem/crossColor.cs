using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossColor : ColorObject
{
    protected override void SetCorrect()
    {
        if(QuestManager.instance.CheckIfComplete("UseShovel") != true) return;
        NewDialogueTrigger dialogueTrigger = new NewDialogueTrigger();
        dialogueTrigger.dialoguePath = "Level1/useWhite";
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Saturation", 0.5f);
        dialogueTrigger.TriggerDialogue();
        
        this.Iscorrect = true;
        base.SetCorrect();

    }
}
