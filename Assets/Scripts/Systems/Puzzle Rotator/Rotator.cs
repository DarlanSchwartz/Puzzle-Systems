using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : SaveableObject
{
    [Space(10)]
    [Header("States and conditions")]
    public int AmountPlaces;
    public Animator anim;
    public bool sendStateAsParameter = false;
    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnRotate;
    [Space(10)]
    [Header("Songs")]
    [Space(10)]
    public AudioSource mainSource;
    public AudioClip RotateSound;
    public void Press()
    {

        anim.SetInteger("State", (anim.GetInteger("State") + 1) <= (AmountPlaces - 1) ? anim.GetInteger("State") + 1 : 0);

        if(sendStateAsParameter)
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), anim.GetInteger("State").ToString());
        }
        else
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnRotate);
        }

        if(mainSource && RotateSound)
        {
            mainSource.PlayOneShot(RotateSound);
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
        anim.SetInteger("State", int.Parse(dataToSave));
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = anim.GetInteger("State").ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
