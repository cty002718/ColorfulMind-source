using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4EndActivator : QuestObjectActivator
{
	Animator animator;
	void Start() {
		animator = gameObject.GetComponent<Animator>();
	}
	public override void OnComplete() {
		HeroController.instance.inventory_open = true;
        SceneInformation.level5 = true;
		animator.SetTrigger("LevelEnd");
	}
}
