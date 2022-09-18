using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeClue : MonoBehaviour
{
    bool showed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (showed) return;
        HeroController hc = other.GetComponent<HeroController>();
        if (hc)
        {
            this.GetComponent<NewDialogueTrigger>().TriggerDialogue();
            showed = true;
        }
    }
}
