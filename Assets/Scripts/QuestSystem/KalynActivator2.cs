using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalynActivator2 : QuestObjectActivator
{
    public override void OnComplete() {
        gameObject.GetComponent<NewDialogueTrigger>().dialoguePath = "Level1/kalyn3";
        List<Item> il = Inventory.instance.GetItemList();
        for(int i=0;i<il.Count;i++) {
        	if(il[i].itemType == Item.ItemType.Heart_down)
        		il.RemoveAt(i);
        }
 		for(int i=0;i<il.Count;i++) {
        	if(il[i].itemType == Item.ItemType.Heart_right)
        		il.RemoveAt(i); 
        }
        for(int i=0;i<il.Count;i++) {
        	if(il[i].itemType == Item.ItemType.Heart_left)
        		il.RemoveAt(i);      
        }
        Item.AddItem(Item.ItemType.Heart, "一個完整的心型墜飾，發出耀眼的藍色光芒");
    }
}