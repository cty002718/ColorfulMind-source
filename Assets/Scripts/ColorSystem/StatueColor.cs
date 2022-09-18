using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueColor : ColorObject
{
    ColorSystem.ColorInfo ci = new ColorSystem.ColorInfo(Color.red, 0, 0.5f, 0.8f, 1);

    protected override void SetCorrect()
    {
        this.Iscorrect = true;

        base.SetCorrect();
        this.ApplyShader(ci);
        if (!QuestManager.instance.CheckIfComplete("Statue")) { this.GetComponent<StatueBehavior>().canPush = true; }
    }
    protected override void UnsetCorrect()
    {
        this.GetComponent<StatueBehavior>().canPush = false;
    }
}
