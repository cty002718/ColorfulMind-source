using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearController : MonoBehaviour
{
    public GameObject statue;
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
    	if(controller != null) {
    		Destroy(statue);
    		Destroy(gameObject);
    	}
	}
}
