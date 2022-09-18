using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankBookActivator1 : QuestObjectActivator
{
    public override void OnComplete()
    {
        gameObject.GetComponent<NewDialogueTrigger>().dialoguePath = "Level1/blankBook2";
        if(!QuestManager.instance.CheckIfComplete("Heart1") || !QuestManager.instance.CheckIfComplete("Heart2"))
        	Item.AddItem(Item.ItemType.Heart_down, "心型碎片的下半部分");
    }
}
