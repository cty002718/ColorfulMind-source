using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineColor : ColorObject
{
    public GameObject up;
    public GameObject down;

    public void Start() {
        this.GetComponent<SpriteRenderer>().material.SetFloat("_Saturation", 0);
    }

    protected override void SetCorrect()
    {
    	up.SetActive(true);
    	down.SetActive(true);
        this.GetComponent<SpriteRenderer>().material.SetFloat("_Saturation", 1);
        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/vine2";
        this.GetComponent<NewDialogueTrigger>().TriggerDialogue();
        this.Iscorrect = true;
    }
}
