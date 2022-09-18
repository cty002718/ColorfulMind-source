using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EmojiController : MonoBehaviour
{
    public void Shock()
    {
        this.GetComponent<Animator>().Play("Surprised", -1, 0f);
    }
}
