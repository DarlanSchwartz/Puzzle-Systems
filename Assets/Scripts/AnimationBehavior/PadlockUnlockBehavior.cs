using UnityEngine;

public class PadlockUnlockBehavior : StateMachineBehaviour
{
    private int unlockHash = Animator.StringToHash("Unlock");
    private int resetHash = Animator.StringToHash("Reset");

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(unlockHash);
        animator.ResetTrigger(resetHash);
    }
}
