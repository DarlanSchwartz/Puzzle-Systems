using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnergyNodeType {EnergySource,Receiver,Conductor}
public enum RotationAxis {X,Y,Z}

public class EnergyNode : SaveableObject
{
    public EnergyNodeType type = EnergyNodeType.Conductor;
    public bool hasEnergy = false;
    [Space(10)]
    [Header("Indicator")]
    public bool useIndicator = false;
    [Space(10)]
    public Material onMat;
    public Material offMat;
    public Renderer indicator;
    [Space(10)]
    [Header("Raycast")]
    public bool usingLines = false;
    public float lineEndOffset = 0.5f;
    public float maxDistance = 0.1f;
    [Space(10)]
    public Transform rayStartRight;
    public Transform rayStartLeft;
    public Transform rayStartUp;
    public Transform rayStartDown;
    [Space(10)]
    [Header("Rotations")]
    public float rotVelocity = 5f;
    public float rotationStep = 90;
    public RotationAxis rotationAxis = RotationAxis.Z;
    [Space(10)]
    [Header("Signals")]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnEnergize;
    public string ParameterValueOnEnergyOff;

    private RaycastHit leftHit;
    private RaycastHit rightHit;
    private RaycastHit upHit;
    private RaycastHit downHit;

    private LineRenderer leftLine;
    private LineRenderer rightLine;
    private LineRenderer upLine;
    private LineRenderer downLine;

    private Transform lastLeftHit;
    private Transform lastRightHit;
    private Transform lastUpHit;
    private Transform lastDownHit;
    private Vector3 rotTarget;


    private void Awake()
    {
        leftLine = rayStartLeft ? rayStartLeft.GetComponent<LineRenderer>() : null;
        rightLine = rayStartRight ? rayStartRight.GetComponent<LineRenderer>() : null;
        upLine = rayStartUp ? rayStartUp.GetComponent<LineRenderer>() : null;
        downLine = rayStartDown? rayStartDown.GetComponent<LineRenderer>() : null;

        lastLeftHit = null;
        lastRightHit = null;
        lastUpHit = null;
        lastDownHit = null;

        rotTarget = transform.localRotation.eulerAngles;

        SetEnergy(hasEnergy);

        indicator.gameObject.SetActive(indicator && useIndicator ? true : false);

        if (useIndicator && indicator && onMat && offMat)
        {
            indicator.material = hasEnergy ? onMat : offMat;
        }
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotTarget), rotVelocity * Time.deltaTime);

        if (type == EnergyNodeType.Receiver || !hasEnergy)
            return;

        LeftRaycast();
        RightRaycast();
        UpRaycast();
        DownRaycast();
    }

    private void OnDrawGizmos()
    {
        float radius = 0.01f;

        if (rayStartRight && rightHit.point != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(rayStartRight.position, rightHit.point);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(rightHit.point, radius);
        }

        if (rayStartLeft && leftHit.point != Vector3.zero) 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(rayStartLeft.position, leftHit.point);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(leftHit.point, radius);
        }

        if (rayStartUp && upHit.point != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(rayStartUp.position, upHit.point);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(upHit.point, radius);
        }

        if (rayStartDown && downHit.point != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(rayStartDown.position, downHit.point);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(downHit.point, radius);
        }
    }

    private void LeftRaycast()
    {
        if (rayStartLeft)
        {
            if (Physics.Raycast(rayStartLeft.position, -transform.right, out leftHit, maxDistance))
            {
                if(leftLine && usingLines)
                {
                    leftLine.SetPosition(1, new Vector3(-Vector3.Distance(transform.position, leftHit.point) + lineEndOffset, 0, 0));
                }

                if (leftHit.transform.tag == "EnergyNode")
                {
                    leftHit.transform.SendMessage("SetEnergy", hasEnergy);

                    if (lastLeftHit != null && leftHit.transform != lastLeftHit.transform)
                    {
                        lastLeftHit.SendMessage("SetEnergy", false);
                    }

                    lastLeftHit = leftHit.transform;
                }
                else
                {
                    if (lastLeftHit)
                    {
                        lastLeftHit.SendMessage("SetEnergy", false);
                        lastLeftHit = null;
                    }
                }
            }
            else
            {
                if (lastLeftHit)
                {
                    lastLeftHit.SendMessage("SetEnergy", false);
                    lastLeftHit = null;
                }

                if (leftLine && usingLines)
                {
                    leftLine.SetPosition(1, new Vector3(-maxDistance, 0, 0));
                }
            }
        }
    }

    private void RightRaycast()
    {
        if (rayStartRight)
        {
            if (Physics.Raycast(rayStartRight.position, transform.right, out rightHit, maxDistance))
            {
                if (rightLine && usingLines)
                {
                    rightLine.SetPosition(1, new Vector3(Vector3.Distance(transform.position, rightHit.point) - lineEndOffset, 0, 0));
                }

                if (rightHit.transform.tag == "EnergyNode")
                {
                    rightHit.transform.SendMessage("SetEnergy", hasEnergy);

                    if (lastRightHit != null && rightHit.transform != lastRightHit.transform)
                    {
                        lastRightHit.SendMessage("SetEnergy", false);
                    }

                    lastRightHit = rightHit.transform;
                }
                else
                {
                    if (lastRightHit)
                    {
                        lastRightHit.SendMessage("SetEnergy", false);
                        lastRightHit = null;
                    }
                }
            }
            else
            {
                if (lastRightHit)
                {
                    lastRightHit.SendMessage("SetEnergy", false);
                    lastRightHit = null;
                }

                if (rightLine && usingLines)
                {
                    rightLine.SetPosition(1, new Vector3(maxDistance, 0, 0));
                }
            }
        }
    }

    private void UpRaycast()
    {
        if (rayStartUp)
        {
            if (Physics.Raycast(rayStartUp.position, transform.up, out upHit, maxDistance))
            {
                if (upLine && usingLines)
                {
                    upLine.SetPosition(1, new Vector3(0, Vector3.Distance(transform.position, upHit.point) - lineEndOffset, 0));
                }

                if (upHit.transform.tag == "EnergyNode")
                {
                    upHit.transform.SendMessage("SetEnergy", hasEnergy);

                    if (lastUpHit != null && upHit.transform != lastUpHit.transform)
                    {
                        lastUpHit.SendMessage("SetEnergy", false);
                    }

                    lastUpHit = upHit.transform;
                }
                else
                {
                    if (lastUpHit)
                    {
                        lastUpHit.SendMessage("SetEnergy", false);
                        lastUpHit = null;
                    }
                }
            }
            else
            {
                if (lastUpHit)
                {
                    lastUpHit.SendMessage("SetEnergy", false);
                    lastUpHit = null;
                }

                if (upLine && usingLines)
                {
                    upLine.SetPosition(1, new Vector3(0, maxDistance, 0));
                }
            }
        }
    }

    private void DownRaycast()
    {
        if (rayStartDown)
        {
            if (Physics.Raycast(rayStartDown.position, -transform.up, out downHit, maxDistance))
            {
                if (downLine && usingLines)
                {
                    downLine.SetPosition(1, new Vector3(0, -Vector3.Distance(transform.position, downHit.point) + lineEndOffset, 0));
                }

                if (downHit.transform.tag == "EnergyNode")
                {
                    downHit.transform.SendMessage("SetEnergy", hasEnergy);

                    if (lastDownHit != null && downHit.transform != lastDownHit.transform)
                    {
                        lastDownHit.SendMessage("SetEnergy", false);
                    }

                    lastDownHit = downHit.transform;
                }
                else
                {
                    if (lastDownHit)
                    {
                        lastDownHit.SendMessage("SetEnergy", false);
                        lastDownHit = null;
                    }
                }
            }
            else
            {
                if (lastDownHit)
                {
                    lastDownHit.SendMessage("SetEnergy", false);
                    lastDownHit = null;
                }

                if (downLine && usingLines)
                {
                    downLine.SetPosition(1, new Vector3(0, -maxDistance, 0));
                }
            }
        }
    }

    public void SetEnergy(bool value)
    {
        if (value == hasEnergy)
            return;

        switch (type)
        {
            case EnergyNodeType.EnergySource:

                hasEnergy = value;

                if (rayStartRight && rightHit.transform && rightHit.transform.tag == "EnergyNode" && rightHit.transform.gameObject.activeSelf)
                {
                    rayStartRight.gameObject.SetActive(value);
                    rightHit.transform.SendMessage("SetEnergy", value);
                }

                if (rayStartLeft && leftHit.transform && leftHit.transform.tag == "EnergyNode" && leftHit.transform.gameObject.activeSelf)
                {
                    rayStartLeft.gameObject.SetActive(value);
                    leftHit.transform.SendMessage("SetEnergy", value);
                }

                if (rayStartUp && upHit.transform && upHit.transform.tag == "EnergyNode" && upHit.transform.gameObject.activeSelf)
                {
                    rayStartUp.gameObject.SetActive(value);
                    upHit.transform.SendMessage("SetEnergy", value);
                }

                if (rayStartDown && downHit.transform && downHit.transform.tag == "EnergyNode" && downHit.transform.gameObject.activeSelf)
                {
                    rayStartDown.gameObject.SetActive(value);
                    downHit.transform.SendMessage("SetEnergy", value);
                }

                break;
            case EnergyNodeType.Receiver:

                hasEnergy = value;

                if(hasEnergy)
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnEnergize);
                }
                else
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnEnergyOff);
                }

                break;
            case EnergyNodeType.Conductor:

                hasEnergy = value;

                if (rayStartRight && rightHit.transform && rightHit.transform.tag == "EnergyNode" && rightHit.transform.gameObject.activeSelf)
                {
                    rayStartRight.gameObject.SetActive(value);
                    rightHit.transform.SendMessage("SetEnergy", value);
                }

                if (rayStartLeft && leftHit.transform && leftHit.transform.tag == "EnergyNode" && leftHit.transform.gameObject.activeSelf)
                {
                    rayStartLeft.gameObject.SetActive(value);
                    leftHit.transform.SendMessage("SetEnergy", value);
                }

                if (rayStartUp && upHit.transform && upHit.transform.tag == "EnergyNode" && upHit.transform.gameObject.activeSelf)
                {
                    rayStartUp.gameObject.SetActive(value);
                    upHit.transform.SendMessage("SetEnergy", value);
                }

                if (rayStartDown && downHit.transform && downHit.transform.tag == "EnergyNode" && downHit.transform.gameObject.activeSelf)
                {
                    rayStartDown.gameObject.SetActive(value);
                    downHit.transform.SendMessage("SetEnergy", value);
                }

                break;
            default:
                break;
        }

        if (indicator && onMat && offMat)
        {
            indicator.material = hasEnergy ? onMat : offMat;
        }

    }
    
    public void Rotate(bool more)
    {
        float xFactor = rotationAxis == RotationAxis.X ? 1 : 0;
        float yFactor = rotationAxis == RotationAxis.Y ? 1 : 0;
        float zFactor = rotationAxis == RotationAxis.Z ? 1 : 0;

        float stepCalc = more ? rotationStep : -rotationStep;

        rotTarget = rotTarget + new Vector3(stepCalc * xFactor, stepCalc * yFactor, stepCalc * zFactor);

        if(rotTarget.x == 360 || rotTarget.y == 360 || rotTarget.z == 360)
        {
            rotTarget = Vector3.zero;
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
        string[] loadedData = dataToSave.Split('|');

        hasEnergy = bool.Parse(loadedData[0]);
        Vector3 loadedRotation = new Vector3(float.Parse(loadedData[1]), float.Parse(loadedData[2]), float.Parse(loadedData[3]));
        Vector3 loadedRotationTarget = new Vector3(float.Parse(loadedData[4]), float.Parse(loadedData[5]), float.Parse(loadedData[6]));
        transform.localRotation = Quaternion.Euler(loadedRotation);
        rotTarget = loadedRotationTarget;
        if (useIndicator && indicator && onMat && offMat)
        {
            indicator.material = hasEnergy ? onMat : offMat;
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        string rotationString = transform.localRotation.eulerAngles.x + "|" + transform.localRotation.eulerAngles.y + "|" + transform.localRotation.eulerAngles.z;
        string rotationTargetString = rotTarget.x + "|" + rotTarget.y + "|" + rotTarget.z;
        dataToSave = hasEnergy.ToString() + "|" + rotationString + "|" + rotationTargetString;
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
