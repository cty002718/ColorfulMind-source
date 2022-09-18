using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueActivator : QuestObjectActivator
{
    public override void OnComplete()
    {
        NewDialogueTrigger dt = GetComponent<NewDialogueTrigger>();
        dt.TriggerDialogue();
        if(!QuestManager.instance.CheckIfComplete("Heart1") || !QuestManager.instance.CheckIfComplete("Heart3"))
        	Item.AddItem(Item.ItemType.Heart_left, "心型碎片的左半部分");
    }
}
