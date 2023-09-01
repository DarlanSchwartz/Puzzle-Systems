using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorManager : SaveableObject
{
    [Space(10)]
    [Header("States and conditions")]
    public int CorrectState;
    public int currentState;
    public bool SendMessageOnStateChanged = true;
    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string parameterValueCorrect;
    public string parameterValueIncorrect;

    public void ChangeState(int to)
    {
        currentState = to;

        if(SendMessageOnStateChanged)
        {
            if (currentState == CorrectState)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueCorrect);
            }
            else
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueIncorrect);
            }
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        currentState = int.Parse(dataToSave);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = currentState.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
