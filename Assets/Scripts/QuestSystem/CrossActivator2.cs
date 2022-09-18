using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossActivator2 : QuestObjectActivator
{
	public override void OnComplete() {
		Item.AddItem(Item.ItemType.Cross, "發光的十字架，似乎真的可以照亮東西");
		Destroy(gameObject);
	}
}
