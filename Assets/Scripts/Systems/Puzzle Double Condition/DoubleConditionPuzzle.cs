using System.Collections.Generic;
using UnityEngine;

public class DoubleConditionPuzzle : SaveableObject
{
    [Space(10)]
    [Header("States and conditions")]
    public List<DoubleConditionItemPlace> places;
    public bool completed = false;
    [Space(10)]
    [Header("Action on complete")]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnComplete;

    public void CheckAll()
    {
        if(!completed)
        {
            foreach (DoubleConditionItemPlace place in places)
            {
                if (place.currentRot != place.correctRot || place.from.currentItemId == -1)
                {
                    completed = false;

                    return;
                }
            }

            completed = true;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnComplete);
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
        completed = bool.Parse(dataToSave);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = completed.ToString();
    }
}