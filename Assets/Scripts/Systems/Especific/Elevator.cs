using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float ElevatorSpeed;
    
    public bool isWorking = true;
    public bool hasEnergy = true;
    public bool isMoving = false;
    public bool goToTarget = false;

    private Vector3 targetLocation;

    public int currentLevel;
    public List<Vector3> levels;
    private int targetLevel;


    [Space(10)]
    [Header("Signals")]

    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueLeverUp;
    public string ParameterValueLeverDown;

    private void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(goToTarget)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocation, ElevatorSpeed * Time.deltaTime);
        }
    }

    public void GotToLevel(int level)
    {
        if(level != currentLevel)
        {
            SetTargetAndGo(levels[level]);
            currentLevel = level;
        }
    }

    public void SetTargetAndGo(Vector3 target)
    {
        targetLocation = target;
        goToTarget = true;
        isMoving = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isMoving && collision.transform.parent != this.transform)
        {
            collision.transform.SetParent(transform);
        }
    }
}
