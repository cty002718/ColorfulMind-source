using ColorSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorObject : MonoBehaviour
{
    [SerializeField]
    protected ColorType cct;                    //Correct color type
    protected SpriteRenderer sr;

    protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr) { sr.material = ColorProfile.instance.Shader; }
        Iscorrect = false;
    }


    //設定顏色
    public void SetColor(ColorType _ct)
    {
        if (!Iscorrect && _ct == cct)
        {
            SetCorrect();
            //Iscorrect = true;
        }
    }


    //回傳是不是正確的顏色
    public bool Iscorrect { get; set; }

    //設定為正確的顏色
    protected virtual void SetCorrect()
    {
    }

    protected virtual void UnsetCorrect() { }

    protected void ApplyShader(ColorInfo ci)
    {
        sr.material.SetColor("color", ci.c);
        sr.material.SetFloat("_Hue", ci.hue);
        sr.material.SetFloat("_Saturation", ci.saturation);
        sr.material.SetFloat("_Brightness", ci.brightness);
        sr.material.SetFloat("_Contrast", ci.contrast);

    }

    public void SetCorrectType(ColorType _ct) { cct = _ct; }
}
