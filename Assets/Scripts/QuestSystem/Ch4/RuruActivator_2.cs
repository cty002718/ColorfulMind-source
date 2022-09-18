using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuruActivator_2 : QuestObjectActivator
{
    public Portal portal;
    [SerializeField]
    string sBgm;

    public override void OnComplete()
    {
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Ruru_cry_2";
        portal.BCanPass = true;
        //AudioController.instance.SmothPlayBgm(sBgm);
    }

}
