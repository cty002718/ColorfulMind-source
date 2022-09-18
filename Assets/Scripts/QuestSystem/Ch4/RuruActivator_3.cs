using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuruActivator_3 : QuestObjectActivator
{
    public GameObject petalPrefab;
    public GameObject envelopePrefab;
    public Vector2 pos1, pos2,pos3;
    private GameObject p1, p2, p3;
    [SerializeField]
    string sBgm;


    public override void OnComplete()
    {
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Ruru_cry_3";
        GenerateStage3();
    }

    private void GenerateStage3()
    {
        p1 = GameObject.Instantiate(petalPrefab);
        p1.transform.position = pos1;
        p2 = GameObject.Instantiate(petalPrefab);
        p2.transform.position = pos2;
        p3 = GameObject.Instantiate(envelopePrefab);
        p3.transform.position = pos3;

        AudioController.instance.SmothPlayBgm(sBgm);
    }
}
