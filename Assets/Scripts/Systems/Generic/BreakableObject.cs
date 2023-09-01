using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public List<Rigidbody> parts;

    public void Break()
    {
        foreach (Rigidbody part in parts)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            part.transform.SetParent(null);
            part.GetComponent<Collider>().enabled = true;
            part.GetComponent<Renderer>().enabled = true;
            part.useGravity = true;
            part.isKinematic = false;
            part.constraints = RigidbodyConstraints.None;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Break();
    }
}
