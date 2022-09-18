using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperController : ItemController
{
    public void UseItem() {
        Debug.Log("read paper");
    	
        SubDialogueTrigger dialogueTrigger = new SubDialogueTrigger();
        dialogueTrigger.dialoguePath = "Level4/readPaper";
        UI_Inventory.instance.gameObject.SetActive(false);
        HeroController.instance.inventory_open = false;
        dialogueTrigger.TriggerDialogue();
    }
}
