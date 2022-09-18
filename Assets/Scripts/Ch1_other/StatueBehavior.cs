using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueBehavior : MonoBehaviour
{
    public bool canPush = false;
    private bool isMoving = false;
    static private float fSpeed = 1.8f;

    private float timer = 0f;
    static private float staytime = 0.1f;

    private void Start()
    {
    }

    void Update()
    {
        CheckLayer();
    }


    //functions
    /*
    威廷2020/10/27 
    功能：把update的功能包起來，修正圖層的修改方式，
    */
    private void CheckLayer()
    {
        if (HeroController.instance.transform.position.y < this.transform.position.y)
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = HeroController.instance.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;
        else
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = HeroController.instance.GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canPush || isMoving) return;
        if (collision.gameObject.tag == "Player" && !isMoving)
        {
            Debug.Log("Do");
            timer += Time.deltaTime;
            if (timer < staytime) return;
            Vector2 v2Center = GetComponent<BoxCollider2D>().bounds.center;
            //Debug.Log(v2Center.ToString());
            Vector2 v2Player = collision.transform.position;
            Vector2 v2Dir = v2Player - v2Center;
            float angle = Vector2.SignedAngle(Vector2.right, v2Dir);
            //Debug.Log(angle.ToString());
            Vector2 v2NewPos = v2Center;
            if (angle < 45 && angle >= -45)
            {
                v2NewPos = v2Center - Vector2.right;
                v2Dir = -Vector2.right;
            }
            else if (angle >= 45 && angle < 135)
            {
                v2NewPos = v2Center - Vector2.up;
                v2Dir = -Vector2.up;
            }
            else if (angle >= 135 || angle < -135)
            {
                v2NewPos = v2Center + Vector2.right;
                v2Dir = Vector2.right;
            }
            else if (angle <= -45 && angle >= -135)
            {
                v2NewPos = v2Center + Vector2.up;
                v2Dir = Vector2.up;
            }


            //check collider
            RaycastHit2D[] rayInfos = Physics2D.RaycastAll(v2Center, v2Dir, 1);
            //Debug.Log(rayInfos.Length);
            foreach (RaycastHit2D ri in rayInfos)
            {
                if (ri.collider.tag == "Pushable" || ri.collider.tag == "CameraConfiner") continue;
                if (!ri.collider.isTrigger)
                {
                    return;
                }
            }

            StartCoroutine(Push(v2Dir));
        }
    }

    private IEnumerator Push(Vector2 v2Dir)
    {
        timer = 0;
        isMoving = true;
        float fProcess = 0;
        Vector2 v2Pos = transform.position;
        while (fProcess <= 1)
        {
            this.transform.position = Vector2.Lerp(v2Pos, v2Pos + v2Dir, fProcess);
            fProcess += fSpeed * Time.deltaTime;
            yield return null;
        }
        this.transform.position = Vector2.Lerp(v2Pos, v2Pos + v2Dir, 1);
        isMoving = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            timer = 0;
        }
    }
}
