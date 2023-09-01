using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiManager : SaveableObject
{
    public bool firstTime = true;
    public bool isCompleted = false;
    public List<EnergyNodeLoki> allLokis;

    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnComplete;

    public void Complete()
    {
        if(!isCompleted)
        {
            isCompleted = true;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnComplete);
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    public void CheckCompletion()
    {
        foreach (EnergyNodeLoki loki in allLokis)
        {
            if (!loki.hasEnergy)
            {
                return;
            }
        }

       Complete();
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
        firstTime = bool.Parse(loadedData[0]);
        isCompleted = bool.Parse(loadedData[1]);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = firstTime.ToString() + "|" + isCompleted.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
