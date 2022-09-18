using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueTarget : MonoBehaviour
{
    public bool OnTarget
    {
        get;
        private set;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pushable"){
            OnTarget = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Pushable")
        {
            OnTarget = false;
        }
    }
}
