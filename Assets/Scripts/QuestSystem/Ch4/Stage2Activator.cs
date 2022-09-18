using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Activator : QuestObjectActivator
{

    public override void OnComplete()
    {
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Ruru_3";
    }


}
