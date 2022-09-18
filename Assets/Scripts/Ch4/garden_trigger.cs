using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garden_trigger : MonoBehaviour
{
	public bool init = false;
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
    	if(controller != null && !init) {
    		init = true;
            this.GetComponent<NewDialogueTrigger>().TriggerDialogue();
    	}
	}
}
