using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Activator : QuestObjectActivator
{
    bool isComplete = false;
    public TextAsset stage3CompleteText;


    public override void OnComplete()
    {
        this.GetComponent<NewDialogueTrigger>().TriggerDialogue(stage3CompleteText);
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Ruru_complete";
    }

    private void Update()
    {
        if (isComplete) return;
        if (!QuestManager.instance.CheckIfComplete("Stage3_Begin")) return;
        if (RemptyTool.ES_MessageSystem.ES_MessageSystem.instance.IsDoingTextTask) return;
        List<Item> itemlist = Inventory.instance.GetItemList();
        
        foreach (Item i in itemlist)
        {
            if (i.itemType == Item.ItemType.Petal && i.amount == 5)
            {
                isComplete = true;
                QuestManager.instance.MarkQuestComplete("Stage3_Complete");
                break;
            }
        }
    }

}
