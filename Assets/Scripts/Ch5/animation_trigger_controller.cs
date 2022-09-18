using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_trigger_controller : MonoBehaviour
{
    bool isTrigger = false;

    void OnTriggerEnter2D(Collider2D other)
	{
    	HeroController controller = other.GetComponent<HeroController>();

    	if (!isTrigger && controller != null)
    	{
            NewDialogueTrigger tr = new NewDialogueTrigger();
            tr.dialoguePath = "Level5/ChasingDialogue";
            tr.TriggerDialogue();
        	isTrigger = true;
    	}
	}
}
