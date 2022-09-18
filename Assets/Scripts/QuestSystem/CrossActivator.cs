using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossActivator : QuestObjectActivator
{

	public override void OnComplete() {
		gameObject.GetComponent<NewDialogueTrigger>().dialoguePath = "Level1/cross2";
	}
}
