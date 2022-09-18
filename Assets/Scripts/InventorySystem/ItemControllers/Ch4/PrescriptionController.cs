using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrescriptionController : ItemController
{
    public void UseItem() {
        Debug.Log("use prescription");
    	
        SubDialogueTrigger dialogueTrigger = new SubDialogueTrigger();
        dialogueTrigger.dialoguePath = "Level4/readPrescription";
        UI_Inventory.instance.gameObject.SetActive(false);
        HeroController.instance.inventory_open = false;
        dialogueTrigger.TriggerDialogue();
    }
}
