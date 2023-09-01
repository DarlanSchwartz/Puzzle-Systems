using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlaceRotatorBehavior : StateMachineBehaviour
{
    public int myStateRotation = 0;
    private DoubleConditionItemPlace target;
    private int resetHash = Animator.StringToHash("Reset");

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.transform.GetComponent<DoubleConditionItemPlace>();

        target.Rotating = true;
        target.CanRotate = false;
        animator.ResetTrigger(resetHash);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!animator.IsInTransition(0))
        {
            target.Rotating = false;
            target.CanRotate = true;
            target.currentRot = myStateRotation;
            target.manager.CheckAll();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target.Rotating = false;
        target.CanRotate = true;
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
