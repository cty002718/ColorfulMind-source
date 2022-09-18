using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerActivator : QuestObjectActivator
{
    public GameObject animation_trigger;
    public GameObject[] flowers = new GameObject[4];
    public Sprite[] fSprites = new Sprite[4];
    public string[] textPath = new string[4];

    bool startCheck = false;

    private FlowerColor[] fc = new FlowerColor[4];

    public override void OnComplete()
    {
        for(int i = 0; i < 4; i++)
        {
            fc[i] = flowers[i].AddComponent(typeof(FlowerColor)) as FlowerColor;
            fc[i].spCorrect = fSprites[i];
            fc[i].textPath = textPath[i];
        }
        fc[0].SetCorrectType(ColorSystem.ColorType.Blue);
        fc[1].SetCorrectType(ColorSystem.ColorType.Green);
        fc[2].SetCorrectType(ColorSystem.ColorType.Yellow);
        fc[3].SetCorrectType(ColorSystem.ColorType.Orange);

        startCheck = true;
    }

    private void Update()
    {
        if (startCheck)
        {
            if (fc[0].Iscorrect && fc[1].Iscorrect && fc[2].Iscorrect && fc[3].Iscorrect)
            {
                startCheck = false;
                Item.AddItem(Item.ItemType.Petal, "鮮紅的花瓣");
                animation_trigger.SetActive(true);
                this.GetComponent<NewDialogueTrigger>().TriggerDialogue();
            }
        }
    }
}
