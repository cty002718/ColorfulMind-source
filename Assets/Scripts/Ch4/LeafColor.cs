using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafColor : ColorObject
{
    public float range = 30;
    public GameObject goLight;

    protected override void SetCorrect()
    {
        //Debug.Log("DO");
        GameObject go = GameObject.Instantiate(goLight);
        go.transform.parent = this.transform;
        go.transform.localPosition = Vector2.zero;
        go.transform.localScale = Vector3.one;
        StartCoroutine(OpenLight(go.GetComponent<Lighting2D.Light2D>()));

        this.GetComponent<NewDialogueTrigger>().dialoguePath = "Level4/Leaf_2";
        this.Iscorrect = true;
    }

    private IEnumerator OpenLight(Lighting2D.Light2D l2d)
    {
        l2d.LightDistance = 2;
        l2d.Intensity = 0.1f;
        l2d.Attenuation = 0.8f;
        float process = 0;
        while (l2d.LightDistance != range && l2d.Intensity != 1)
        {
            l2d.LightDistance = Mathf.Lerp(2, range, process);
            l2d.Intensity = Mathf.Lerp(0.1f, 1, process);
            process += Time.deltaTime * 0.83f;
            yield return null;
        }

        //StartCoroutine(Shine(l2d));
    }

    private IEnumerator Shine(Lighting2D.Light2D l2d)
    {
        float value = 1;
        while (true)
        {
            l2d.LightDistance += Time.deltaTime * value * 5;
            l2d.Intensity = Mathf.Pow(l2d.LightDistance / range, 3);
            if (l2d.LightDistance > (range + 2)) value = -1;
            else if (l2d.LightDistance < (range -2)) value = 1;
            yield return null;
        }
    }
}
