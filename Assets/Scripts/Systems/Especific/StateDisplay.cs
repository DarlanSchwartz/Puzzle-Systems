using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateDisplay : MonoBehaviour
{

    public Openable door;
    public Text displayText;
    public Light displayLight;
    public bool ShowLockState = true;

    void Update()
    {
        if(!ShowLockState)
        {
            if (door.isOpen)
            {
                displayText.text = "Open";
                displayText.color = Color.green;
                displayLight.color = Color.green;
            }
            else
            {
                displayText.text = "Closed";
                displayText.color = Color.red;
                displayLight.color = Color.red;
            }
        }
        else
        {
            if (!door.isLocked)
            {
                displayText.text = "Unlocked";
                displayText.color = Color.green;
                displayLight.color = Color.green;
            }
            else
            {
                displayText.text = "Locked";
                displayText.color = Color.red;
                displayLight.color = Color.red;
            }
        }

    }
}
