using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoActivator2 : QuestObjectActivator
{
	[SerializeField] protected Portal p;

	public override void OnComplete() {
		
		gameObject.GetComponent<NewDialogueTrigger>().dialoguePath = "Level1/piano3";
		if(!QuestManager.instance.CheckIfComplete("Heart2") || !QuestManager.instance.CheckIfComplete("Heart3"))
			Item.AddItem(Item.ItemType.Heart_right, "心型碎片的右半部分");

		// play the music	
		AudioController.instance.SmothPlayBgm("1-3-1");
		p.audioName = "1-3-1";
	}
}

