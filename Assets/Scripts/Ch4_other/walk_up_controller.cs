using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk_up_controller : MonoBehaviour
{
	float timer = 0;
	float touchTime = 0;

	void Update() {
		if(timer >= 0)
			timer -= Time.deltaTime;
	}

	void OnTriggerStay2D(Collider2D other)
	{
    	HeroController controller = other.GetComponent<HeroController>();

		touchTime += Time.deltaTime;
		if (controller != null && controller.lookDirection.y == 1 && timer <= 0 && touchTime > 0.2f)
    	{
    		timer = 1;
			controller.animator.SetFloat("AX", 0);
			controller.animator.SetFloat("AY", 1);
			controller.animator.SetFloat("AT", 1.8f);
			controller.animator.SetFloat("AS", 1.5f);
			controller.animator.Play("Base Layer.AnimeMove");
			touchTime = 0;
		}
	}

}
