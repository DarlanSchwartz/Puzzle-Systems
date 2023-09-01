using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : SaveableObject
{
    [Header("States and conditions")]
    [Space(10)]
    public bool hasEnergy;
    public bool isEnabled;
    public bool hasEnoughtFuses;
    public int neededFuses;
    public int activeFuses;
    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueCompleted;
    public string ParameterValueNotCompleted;

    private void OnDrawGizmos()
    {
        
    }

    public void SetEnergyState(string state)
    {
        if (state == "Energized")
        {
            hasEnergy = true;

            if(hasEnoughtFuses && enabled)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCompleted);
            }
        }
        else if (state == "NotEnergized")
        {
            hasEnergy = false;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueNotCompleted);
        }
    }

    public void SetEnabledState(string state)
    {
        if(state == "Enabled")
        {
            isEnabled = true;

            if (hasEnoughtFuses && hasEnergy)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCompleted);
            }
        }
        else if(state == "Disabled")
        {
            isEnabled = false;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueNotCompleted);
        }
    }

    public void AddFuse(int amount)
    {
        activeFuses += amount;

        if(activeFuses == neededFuses)
        {
            hasEnoughtFuses = true;

            if (isEnabled && hasEnergy)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCompleted);
            }
        }
        else
        {
            hasEnoughtFuses = false;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueNotCompleted);
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
        hasEnergy = bool.Parse(dataToSave);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = hasEnergy.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
