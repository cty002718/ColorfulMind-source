using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleActivator1 : QuestObjectActivator
{
	public GameObject capsule;
	public override void OnComplete() {
		capsule.SetActive(true);
	}
}
