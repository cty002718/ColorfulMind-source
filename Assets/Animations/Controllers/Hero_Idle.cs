using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Idle : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckDirection(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckDirection(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

    private void CheckDirection(Animator animator)
    {
        if (animator.GetBool("Direction") == false) return;
        //Debug.Log("do  " + Time.time + "  " + animator.GetFloat("Look X") + ", " + animator.GetFloat("Look Y") + ", " + animator.GetBool("Direction"));
        float x = animator.GetFloat("AX");
        float y = animator.GetFloat("AY");
        Vector2 v2Dir = new Vector2(x, y).normalized;
        //Debug.Log(v2Dir.ToString() + Time.deltaTime);
        HeroController.instance.lookDirection = v2Dir;

        animator.SetBool("Direction", false);
    }

}
