using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieFlowerActivator : QuestObjectActivator
{
	public override void OnComplete() {
		Item.AddItem(Item.ItemType.Petal, "鮮紅的花瓣");
		gameObject.GetComponent<dieFlowerColor>().Iscorrect = true;
	}
}