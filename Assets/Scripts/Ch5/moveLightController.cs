using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLightController : MonoBehaviour
{
    public GameObject PathFinding;
    public float speed;
    public float timer;
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveUp() {
        Vector2 position = tr.position;
        while(timer >= 0) {
            position.y += speed * Time.deltaTime;
            tr.position = position;
            timer -= Time.deltaTime;
            yield return null;
        }
        PathFinding.GetComponent<AstarPath>().Scan();
    }
}
