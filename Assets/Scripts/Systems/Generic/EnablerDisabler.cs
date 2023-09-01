using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablerDisabler : MonoBehaviour
{
    public bool isEnabled;
    [Space(5)]
    [Header("Turning on")]
    [Space(5)]
    public List<Transform> TurnOnEnableObjects;
    public List<Transform> TurnOnDisableObjects;
    [Space(5)]
    [Header("Turning off")]
    [Space(5)]
    public List<Transform> TurnOffEnableObjects;
    public List<Transform> TurnOffDisableObjects;


    public void Press()
    {
        Interact();
    }

    public void Interact()
    {
        if(isEnabled)
        {
            DisableObjects();
        }
        else
        {
            EnableObjects();
        }
    }

    public void EnableObjects()
    {
        if (TurnOnEnableObjects.Count > 0)
        {
            foreach (Transform toEnableObject in TurnOnEnableObjects)
            {
                toEnableObject.gameObject.SetActive(true);
            }
        }

        if (TurnOnDisableObjects.Count > 0)
        {
            foreach (Transform toDisableObject in TurnOnDisableObjects)
            {
                toDisableObject.gameObject.SetActive(false);
            }
        }

        isEnabled = true;
    }

    public void DisableObjects()
    {
        if (TurnOffEnableObjects.Count > 0)
        {
            foreach (Transform toEnableObject in TurnOffEnableObjects)
            {
                toEnableObject.gameObject.SetActive(true);
            }
        }

        if (TurnOffDisableObjects.Count > 0)
        {
            foreach (Transform toDisableObject in TurnOffDisableObjects)
            {
                toDisableObject.gameObject.SetActive(false);
            }
        }

        isEnabled = false;
    }
}
