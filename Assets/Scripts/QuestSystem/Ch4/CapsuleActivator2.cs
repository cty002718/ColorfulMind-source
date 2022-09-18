using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleActivator2 : QuestObjectActivator
{
	public override void OnComplete() {
		Destroy(GetComponent("NewDialogueTrigger"));
		SubDialogueTrigger sub = this.gameObject.AddComponent(typeof(SubDialogueTrigger)) as SubDialogueTrigger;
		sub.dialoguePath = "Level4/capsule2";
	}
}
