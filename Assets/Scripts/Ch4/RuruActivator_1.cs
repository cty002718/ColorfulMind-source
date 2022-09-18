using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuruActivator_1 : QuestObjectActivator
{
    public GameObject petalPrefab;
    public Vector2 pos1, pos2;
    private GameObject p1, p2;

    public override void OnComplete()
    {
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Ruru_cry";
        GenerateStage1();
    }

    private void GenerateStage1()
    {
        p1 = GameObject.Instantiate(petalPrefab);
        p1.transform.position = pos1;
        p2 = GameObject.Instantiate(petalPrefab);
        p2.transform.position = pos2;
    }
}
