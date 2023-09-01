using UnityEngine;

public class ResetTriggerResetBehavior : StateMachineBehaviour
{
    private int resetHash = Animator.StringToHash("Reset");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(resetHash);
    }
}