using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Activator : QuestObjectActivator
{
    bool isComplete = false;
    public TextAsset stage1CompleteText;


    public override void OnComplete()
    {
        this.GetComponent<NewDialogueTrigger>().TriggerDialogue(stage1CompleteText);
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Ruru_2";
    }

    private void Update()
    {
        if (isComplete) return;
        if (!QuestManager.instance.CheckIfComplete("Stage1_Begin")) return;
        if (RemptyTool.ES_MessageSystem.ES_MessageSystem.instance.IsDoingTextTask) return;
        List<Item> itemlist = Inventory.instance.GetItemList();
        
        foreach (Item i in itemlist)
        {
            if (i.itemType == Item.ItemType.Petal && i.amount == 3)
            {
                isComplete = true;
                QuestManager.instance.MarkQuestComplete("Stage1_Complete");
                break;
            }
        }
    }

}
