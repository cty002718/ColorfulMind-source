using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlowerColor : ColorObject
{
    public Sprite spCorrect;
    public string textPath;


    protected override void SetCorrect()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps) ps.Play();
        Invoke("ChangeAttributes", 0.2f);
        this.Iscorrect = true;
    }

    private void ChangeAttributes()
    {
        this.GetComponent<SpriteRenderer>().sprite = spCorrect;
        this.GetComponent<NewDialogueTrigger>().dialoguePath = textPath;
    }
}
