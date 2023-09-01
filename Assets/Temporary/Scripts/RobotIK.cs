using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIK : MonoBehaviour
{
    public float SamplingDistance = 0.5f;
    public float LearningRate = 1f;
    public float DistanceThreshold = 1;
    public RobotJoint[] Joints;

   public float PartialGradient (Vector3 target, float[] angles, int i)
    {
        float angle = angles[i];

        float f_x = DistanceFromTarget(target, angles);

        angles[i] += SamplingDistance;

        float f_X_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_X_plus_d - f_x) / SamplingDistance;

        angles[i] = angle;

        return gradient;
    }

    private void OnDrawGizmos()
    {
        
    }

    private float DistanceFromTarget(Vector3 target, float[] angles)
    {
        throw new NotImplementedException();
    }

    public void InverseKinematics(Vector3 target,float[] angles)
    {
        if (DistanceFromTarget(target, angles) < DistanceThreshold)
            return;

        for (int i = 0; i < Joints.Length; i++)
        {
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= LearningRate * gradient;

            angles[i] = Mathf.Clamp(angles[i], Joints[i].minAngle, Joints[i].maxAngle);

            if (DistanceFromTarget(target, angles) < DistanceThreshold)
                return;
        }
    }
}
