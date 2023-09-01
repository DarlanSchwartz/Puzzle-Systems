using UnityEngine;

public class LaserReceiver : SaveableObject
{
    public bool hasEnergy;

    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string parameterValueEnergized;
    public string parameterValueNotEnergized;

    

    public void LaserHit()
    {
        if(!hasEnergy)
        {
            hasEnergy = true;
            Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueEnergized);
        }
    }

    public void LaserOut()
    {
        if(hasEnergy)
        {
            hasEnergy = false;
            Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueNotEnergized);
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
