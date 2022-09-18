using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleActivator3 : QuestObjectActivator
{
	public GameObject flower;
	public override void OnComplete() {
		Item.AddItem(Item.ItemType.Petal, "鮮紅的花瓣");
	}
}
