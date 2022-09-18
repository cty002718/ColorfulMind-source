using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RemptyTool.ES_MessageSystem;

public class SubDialogueTrigger : NewDialogueTrigger
{
    public override void TriggerDialogue()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Dialogues/" + dialoguePath);
        ES_MessageSystem.instance.BeginTextTask_2(this, textAsset);
    }
}
