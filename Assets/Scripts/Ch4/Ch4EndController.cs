using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch4EndController : MonoBehaviour
{
    [SerializeField]
    protected GameObject reaperPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HeroController>() != null)
        {
            GameObject go = GameObject.Instantiate(reaperPrefab);
            go.name = "Death";
            go.transform.parent = transform;
            go.transform.localPosition = new Vector3(7, 15, 0);
            this.GetComponent<NewDialogueTrigger>().TriggerDialogue();
            GameObject.Destroy(this.GetComponent<Collider2D>());
        }
    }
}
