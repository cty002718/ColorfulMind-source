using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeStatueActivator : QuestObjectActivator
{
    bool isComplete = false;
    [SerializeField]
    Portal portal;

    public override void OnComplete()
    {
        Item.AddItem(Item.ItemType.Petal, "鮮紅的花瓣");
        Item.AddItem(Item.ItemType.Key, "一把不知道用在哪的鑰匙");
        //傳送去中央地圖

    }

    private void Update()
    {
        if (isComplete) return;
        if (!QuestManager.instance.CheckIfComplete("Stage2_Begin")) return;
        if (RemptyTool.ES_MessageSystem.ES_MessageSystem.instance.IsDoingTextTask) return;
        List<Item> itemlist = Inventory.instance.GetItemList();
        
        foreach (Item i in itemlist)
        {
            if (i.itemType == Item.ItemType.Petal && i.amount == 3)
            {
                isComplete = true;
                portal.BCanPass = false;
                portal.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Maze_door_2";
                this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/statue2";
                break;
            }
        }

        
    }

}
