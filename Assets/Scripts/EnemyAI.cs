using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    AIPath aiPath;
    public Animator animator;

    void Start() {
        aiPath = this.GetComponent<AIPath>();
    }

    void Update() {
        Vector3 tmp = transform.localScale;
        if(aiPath.desiredVelocity.x >= 0.01f) {
            tmp.x = -Math.Abs(tmp.x);
            transform.localScale = tmp;
        } else if(aiPath.desiredVelocity.x <= -0.01f) {
            tmp.x = Math.Abs(tmp.x);
            transform.localScale = tmp;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        HeroController hero = other.gameObject.GetComponent<HeroController>();

        if(hero != null) {
            AudioController.instance.Mute(0.3f);
            hero.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            hero.inventory_open = true;
            SceneInformation.chasing = true;
            animator.SetTrigger("FadeOut");
        }
    }
}
