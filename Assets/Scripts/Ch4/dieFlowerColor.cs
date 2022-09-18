using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieFlowerColor : ColorObject
{

	void Start() {
		base.Start();
		sr.material.SetFloat("_Saturation", 0);
	}

    protected override void SetCorrect()
    {
        if (!QuestManager.instance.CheckIfComplete("Stage3_Begin")) return;
    	NewDialogueTrigger dialogueTrigger = new NewDialogueTrigger();
        dialogueTrigger.dialoguePath = "Level4/dieFlowerTrigger";
        UI_Inventory.instance.gameObject.SetActive(false);
        HeroController.instance.inventory_open = false;
        dialogueTrigger.TriggerDialogue();
    }
}
