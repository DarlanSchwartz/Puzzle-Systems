using UnityEngine;
using System.Collections;

public class DoubleConditionItemPlace : SaveableObject
{
    public ItemPlace from;
    public DoubleConditionPuzzle manager;
    public Animator anim;

    public int currentRot;
    public int correctRot = 0;
    public bool Rotating { get; set; }
    public bool CanRotate { get; set; } = true;

    private int animTargetHash = Animator.StringToHash("Target");
    private int resetTriggerHash = Animator.StringToHash("Reset");

    public void SetEnabledState(bool enable)
    {
        Rotating = false;
        CanRotate = true;
       
        if(enable)
        {
            manager.CheckAll();
        }
        else
        {
            anim.SetInteger(animTargetHash, 0);
            anim.SetTrigger(resetTriggerHash);
            currentRot = 0;
        }

        anim.ResetTrigger(resetTriggerHash);
    }

    public void RotateMethod(bool more)
    {
       if(CanRotate && from.currentItemId != -1)
        {
            if(!Rotating)
            {
                int animTarget = anim.GetInteger(animTargetHash);
                int target = more ? animTarget + 90 : animTarget - 90;
                
                if(more && animTarget == 270)
                {
                    target = 0;
                    anim.SetInteger(animTargetHash, target);
                    return;
                }

                if(!more && animTarget == 0)
                {
                    target = 270;
                    anim.SetInteger(animTargetHash, target);
                    return;
                }

                anim.SetInteger(animTargetHash, target);
            }
        }
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        Rotating = false;
        CanRotate = true;
        currentRot = int.Parse(dataToSave);
        anim.SetInteger(animTargetHash, int.Parse(dataToSave));
        anim.SetTrigger(resetTriggerHash);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = anim.GetInteger(animTargetHash).ToString();
    }

    private void OnDrawGizmos()
    {
        
    }
}
