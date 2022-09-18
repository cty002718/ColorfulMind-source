using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvelopeActivator : QuestObjectActivator
{
	public override void OnComplete() {
		Item.AddItem(Item.ItemType.Prescription, "處方籤");
		Item.AddItem(Item.ItemType.Paper, "一張皺皺的紙條，上面似乎寫著一些提示");
		Item.AddItem(Item.ItemType.Petal, "鮮紅的花瓣");
		Destroy(gameObject);
	}
}
