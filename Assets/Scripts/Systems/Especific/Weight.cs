using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PressurePlate")
        {
            collision.transform.SendMessage("Enable");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "PressurePlate")
        {
            collision.transform.SendMessage("Enable");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "PressurePlate")
        {
            collision.transform.SendMessage("Disable");
        }
    }
}
