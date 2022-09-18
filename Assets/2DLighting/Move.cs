using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float fSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * fSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3)Vector3.right * Time.deltaTime * fSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= (Vector3)Vector3.up * Time.deltaTime * fSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= (Vector3)Vector3.right * Time.deltaTime * fSpeed;
        }

        if (Input.GetKey(KeyCode.R))
        {
            RenderSettings.ambientLight = Color.black;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            RenderSettings.ambientLight = Color.white;
        }
        else if (Input.GetKey(KeyCode.V))
        {
            RenderSettings.ambientLight = Color.gray;
        }
    }
}
