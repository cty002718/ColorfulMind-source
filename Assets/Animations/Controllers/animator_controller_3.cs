using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animator_controller_3 : StateMachineBehaviour
{
    Vector2 v2Dir = Vector2.up;
    float speed = 1f;
    float remain = 1f;
    float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0f;
        speed = animator.GetFloat("AS");
        remain = animator.GetFloat("AT");
        v2Dir = new Vector2(animator.GetFloat("AX"), animator.GetFloat("AY")).normalized;
        //Debug.Log("速度：" + speed + "  持續：" + remain);
        Collider2D c2 = animator.GetComponent<Collider2D>();
        if (c2) c2.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position += (Vector3)v2Dir * Time.deltaTime * speed;
        time += Time.deltaTime;
        if (time > remain) { animator.SetTrigger("Return"); }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider2D c2 = animator.GetComponent<Collider2D>();
        if (c2) c2.enabled = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
