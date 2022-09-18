using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeSee : MonoBehaviour
{
    [SerializeField]
    NewDialogueTrigger ruru;
    float speed = 3.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Meet());
            GameObject.Destroy(this.GetComponent<BoxCollider2D>());
        }
    }
    private IEnumerator Meet()
    {
        HeroController.instance.isGameStop = true;
        HeroController.instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", 0);
        yield return new WaitForSeconds(0.5f);
        HeroController.instance.transform.GetComponentInChildren<EmojiController>().Shock();
        
        yield return new WaitForSeconds(1.5f);
        Vector2 dist = transform.position - HeroController.instance.transform.position;
        if (dist.x > 0) { dist.x -= 1;  }
        else { dist.x += 1;  }
        //Debug.Log(dist);
        float time1 = Mathf.Abs(dist.x / speed);
        float time2 = Mathf.Abs(dist.y / speed);
        //Debug.Log(dist);
        //Debug.Log(time1.ToString() + ", " + time2.ToString());
        Animator a = HeroController.instance.transform.GetChild(0).GetComponent<Animator>();
        a.Play("Base Layer.AnimeMove");
        a.SetFloat("AX", dist.x);
        a.SetFloat("AY", 0);
        a.SetFloat("AS", speed);
        a.SetFloat("AT", time1);
        yield return new WaitForSeconds(time1 + 0.1f);
        a.Play("Base Layer.AnimeMove");
        a.SetFloat("AX", 0);
        a.SetFloat("AY", dist.y);
        a.SetFloat("AS", speed);
        a.SetFloat("AT", time2);
        yield return new WaitForSeconds(time2 + 0.1f);

        HeroController.instance.isGameStop = false;
        yield return null;
        if (transform.position.x > HeroController.instance.transform.position.x)
        {
            a.SetBool("Direction", true);
            a.SetFloat("AX", 1);
            a.SetFloat("AY", 0);
            //Debug.Log("do1");
            ruru.GetComponent<Animator>().SetFloat("Look X", -1);
            ruru.GetComponent<Animator>().SetFloat("Look Y", 0);
        }
        else
        {
            a.SetBool("Direction", true);
            a.SetFloat("AX", -1);
            a.SetFloat("AY", 0);
            //Debug.Log("Do2");
            ruru.GetComponent<Animator>().SetFloat("Look X", 1);
            ruru.GetComponent<Animator>().SetFloat("Look Y", 0);
        }
        
        
        ruru.TriggerDialogue();
    }
}
