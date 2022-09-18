using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemColoring : ColorObject
{
    [SerializeField]
    private string alternativeDialoguePath;
    [SerializeField]
    private string eventDialoguePath;
    [SerializeField]
    protected float brightness = 0.5f;

    void Start()
    {
        base.Start();
        sr.material.SetFloat("_Saturation", 0);
        sr.material.SetFloat("_Brightness", brightness);
    }

    protected override void SetCorrect()
    {
        NewDialogueTrigger dialogueTrigger = gameObject.GetComponent<NewDialogueTrigger>();
        // Disposable Event dialogue
        dialogueTrigger.dialoguePath = eventDialoguePath;
        dialogueTrigger.TriggerDialogue();
        sr.material.SetFloat("_Saturation", 0.5f);
        sr.material.SetFloat("_Brightness", 0.5f);

        dialogueTrigger.dialoguePath = alternativeDialoguePath;
        // This can be set after some dialogue finished.
        Iscorrect = true;
    }
}
