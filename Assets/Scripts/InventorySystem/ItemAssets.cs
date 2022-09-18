using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    public Sprite bookSprite;
    public Sprite heart_left_Sprite;
    public Sprite heart_right_Sprite;
    public Sprite heart_down_Sprite;
    public Sprite heartSprite;
    public Sprite shovelSprite;
    public Sprite whiteSprite;
    public Sprite crossSprite;
    public Sprite prescriptionSprite;
    public Sprite paperSprite;
    public Sprite petalSprite;
    public Sprite keySprite;

    public string bookDialoguePath;
    public string shovelDialoguePath;
    public string petalDialoguePath;

}
