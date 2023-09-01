using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;

    public float maxAngle;
    public float minAngle;

    private void Awake()
    {
        StartOffset = transform.localPosition;
    }
}
