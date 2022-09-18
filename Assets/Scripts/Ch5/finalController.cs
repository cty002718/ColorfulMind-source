using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalController : MonoBehaviour
{
    public Animator animator;
    public GameObject death;
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
            controller.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            death.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetTrigger("FadeOutWhite");
    	}
	}
}
