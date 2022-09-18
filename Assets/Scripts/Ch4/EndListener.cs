using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndListener : QuestObjectActivator
{
    [SerializeField]
    protected GameObject endObject;
    public override void OnComplete()
    {
        endObject.SetActive(true);
    }
}
