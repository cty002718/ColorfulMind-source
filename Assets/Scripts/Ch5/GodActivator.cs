using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GodActivator : QuestObjectActivator
{
	public override void OnComplete() {
		this.GetComponent<AIDestinationSetter>().enabled = true;
	}
}