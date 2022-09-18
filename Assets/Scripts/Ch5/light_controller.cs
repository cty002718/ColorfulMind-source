using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_controller : MonoBehaviour
{
    bool isTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
	{
    	HeroController controller = other.GetComponent<HeroController>();

    	if (!isTrigger && controller != null)
    	{
        	GameObject light = transform.Find("moveLight").gameObject;
            StartCoroutine(light.GetComponent<moveLightController>().MoveUp());
        	isTrigger = true;
    	}
	}
}
