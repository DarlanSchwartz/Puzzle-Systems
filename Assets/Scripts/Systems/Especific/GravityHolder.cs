using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHolder : SaveableObject
{
    public Rigidbody RigidSet;
    public HingeJoint hinge;
    public bool CanUse = true;
    public bool Disconnect;

    public void ChangeGravityState()
    {
        if(!Disconnect)
        {
            if (CanUse)
            {
                RigidSet.useGravity = !RigidSet.useGravity;
                CanUse = false;
                RigidSet = null;
            }
        }
        else
        {
            if (CanUse)
            {
                Destroy(hinge);
                CanUse = false;
                RigidSet = null;
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
        string[] loadedData = dataToSave.Split('|');

        CanUse = bool.Parse(loadedData[0]);

        bool connectedObject = bool.Parse(loadedData[1]);

        if(!connectedObject)
        {
            Destroy(RigidSet.gameObject);
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        bool connectedObject = RigidSet == null ? false : true;

        dataToSave = CanUse.ToString() + "|" + connectedObject.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
