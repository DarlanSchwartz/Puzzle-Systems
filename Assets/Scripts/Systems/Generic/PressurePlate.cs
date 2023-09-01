using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isEnabled;
    [Space(10)]
    [Header("Signals")]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string parameterValuePressed;
    public string parameterValueUnPressed;
    [Space(10)]
    [Header("Sounds")]
    public AudioSource mainSource;
    public AudioClip enableSound;
    public AudioClip disableSound;

    public void Enable()
    {
        if(!isEnabled)
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValuePressed);

            if (mainSource != null && enableSound != null && !isEnabled)
            {
                mainSource.PlayOneShot(enableSound);
            }

            isEnabled = true;
        }
    }

    public void Disable()
    {
        if(isEnabled)
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueUnPressed);

            if (mainSource != null && disableSound != null)
            {
                mainSource.PlayOneShot(disableSound);
            }

            isEnabled = false;
        }
    }

    public enum MessageType { FloatingNumber, Integer, String, VoidRun }

    private void OnTriggerStay(Collider other)
    {
        Enable();
    }

    private void OnTriggerExit(Collider other)
    {
        Disable();
    }
}
