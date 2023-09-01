using System.Collections;
using UnityEngine;

public class SliderPuzzlePiece : SaveableObject
{

    public bool canMove;
    private bool canMoveLeft;
    private bool canMoveRight;
    private bool canMoveUp;
    private bool canMoveDown;
    public bool isMoving;
    [SerializeField] private SliderPuzzle manager;
    public Vector3 correctPlace;
    [Range(0,0.1f)][SerializeField] private float correctPlaceErrorMargin = 0.01f;
    [SerializeField] private float rayDistance = 1f;
    public float DistanceFactor = 0.5f;
    

    public bool IsInPlace()
    {
        return Vector3.Distance(transform.localPosition, correctPlace) < correctPlaceErrorMargin;
    }

    [ContextMenu("Correct place is my place.")]
    private void SetCorrectPlace()
    {
        correctPlace = transform.localPosition;
    }

    [Space(10)]
    [Header("Sounds")]
    [Space(10)]
    public AudioSource mainSource;
    public AudioClip moveSound;
    private Vector3 targetPos;
    
    RaycastHit hitLeft;
    RaycastHit hitRight;
    RaycastHit hitUp;
    RaycastHit hitDown;

    Vector3 rayStartLeft;
    Vector3 rayStartRight;
    Vector3 rayStartUp;
    Vector3 rayStartDown;

    public bool canInteract = true;

    void Update()
    {
        if (!canInteract)
            return;

        rayStartLeft = (transform.position + (-transform.right * DistanceFactor)) + transform.forward * DistanceFactor;
        rayStartRight = (transform.position + (transform.right * DistanceFactor)) + transform.forward * DistanceFactor;
        rayStartUp = (transform.position + (transform.up * DistanceFactor)) + transform.forward * DistanceFactor;
        rayStartDown = (transform.position + (-transform.up * DistanceFactor)) + transform.forward * DistanceFactor;

        canMoveLeft = Physics.Raycast(rayStartLeft, -transform.forward, out hitLeft, rayDistance) && hitLeft.transform.tag != "SliderPuzzlePiece" && hitLeft.transform.tag == "BackgroundSliderPuzzle";
        canMoveRight = Physics.Raycast(rayStartRight, -transform.forward, out hitRight, rayDistance) && hitRight.transform.tag != "SliderPuzzlePiece" && hitRight.transform.tag == "BackgroundSliderPuzzle";
        canMoveUp = Physics.Raycast(rayStartUp, -transform.forward, out hitUp, rayDistance) && hitUp.transform.tag != "SliderPuzzlePiece" && hitUp.transform.tag == "BackgroundSliderPuzzle";
        canMoveDown = Physics.Raycast(rayStartDown, -transform.forward, out hitDown, rayDistance) && hitDown.transform.tag != "SliderPuzzlePiece" && hitDown.transform.tag == "BackgroundSliderPuzzle";

        canMove = (canMoveLeft || canMoveRight || canMoveUp || canMoveDown);
    }

    private void OnDrawGizmos()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        rayStartLeft = (transform.position + (-transform.right * DistanceFactor)) + transform.forward * DistanceFactor;
        rayStartRight = (transform.position + (transform.right * DistanceFactor)) + transform.forward * DistanceFactor;
        rayStartUp = (transform.position + (transform.up * DistanceFactor)) + transform.forward * DistanceFactor;
        rayStartDown =(transform.position + (-transform.up * DistanceFactor)) + transform.forward * DistanceFactor;

        float radiusP = 0.02f;

        Gizmos.DrawSphere(rayStartLeft, radiusP);
        Gizmos.DrawSphere(rayStartRight, radiusP);
        Gizmos.DrawSphere(rayStartUp, radiusP);
        Gizmos.DrawSphere(rayStartDown, radiusP);

        Gizmos.color = new Color(255,0, 255);

        Physics.Raycast(rayStartLeft, -transform.forward, out hitLeft, rayDistance);
        Physics.Raycast(rayStartRight, -transform.forward, out hitRight, rayDistance);
        Physics.Raycast(rayStartUp, -transform.forward, out hitUp, rayDistance);
        Physics.Raycast(rayStartDown, -transform.forward, out hitDown, rayDistance);

        Gizmos.DrawLine(rayStartLeft, hitLeft.point == Vector3.zero ? rayStartLeft : hitLeft.point );
        Gizmos.DrawLine(rayStartRight, hitRight.point == Vector3.zero ? rayStartRight : hitRight.point);
        Gizmos.DrawLine(rayStartUp, hitUp.point == Vector3.zero ? rayStartUp : hitUp.point);
        Gizmos.DrawLine(rayStartDown, hitDown.point == Vector3.zero ? rayStartDown : hitDown.point);

        Gizmos.color = Color.green;

        Gizmos.DrawSphere(hitLeft.point, radiusP/ 2);
        Gizmos.DrawSphere(hitRight.point, radiusP/ 2);
        Gizmos.DrawSphere(hitUp.point, radiusP / 2);
        Gizmos.DrawSphere(hitDown.point, radiusP /2);
    }

    [ContextMenu("Move")]
    public void Move()
    {
        if (canMove && !isMoving && canInteract)
        {
            if(mainSource && moveSound)
            {
                mainSource.PlayOneShot(moveSound);
            }

            if (canMoveLeft)
            {
                targetPos = hitLeft.point;
                StartCoroutine(MoveToTarget());
                return;
            }

            if (canMoveRight)
            {
                targetPos = hitRight.point;
                StartCoroutine(MoveToTarget());
                return;
            }

            if (canMoveDown)
            {
                targetPos = hitDown.point;
                StartCoroutine(MoveToTarget());
                return;
            }

            if (canMoveUp)
            {
                targetPos = hitUp.point;

                //This could be hit.transform.positions, but it will be needed more gameobjects with colliders and tags,
                //then the raycast dont need to be at the exactly center of the picture

                StartCoroutine(MoveToTarget());
                return;
            }
        }
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;

        float timePassed = 0;
        float speed = manager.pieceMoveSpeed;

        do
        {
            //transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            timePassed += Time.deltaTime;
            if(timePassed > 5)
            {
                break;
            }
            yield return null;
        } while (Vector3.Distance(transform.position,targetPos) > 0.0001f);

        isMoving = false;

        manager.CheckCompletion();
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
        canInteract = bool.Parse(loadedData[0]);
        transform.localPosition = new Vector3(float.Parse(loadedData[1]), float.Parse(loadedData[2]), float.Parse(loadedData[3]));
        isMoving = bool.Parse(loadedData[4]);
        targetPos = new Vector3(float.Parse(loadedData[5]), float.Parse(loadedData[6]), float.Parse(loadedData[7]));

        if(isMoving)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        string positionString = transform.localPosition.x + "|" + transform.localPosition.y + "|" + transform.localPosition.z + "|";
        string targetPosition = "|" + targetPos.x + "|" + targetPos.y + "|" + targetPos.y;
        dataToSave = canInteract.ToString() + "|" + positionString + isMoving.ToString() + targetPosition;
        //                  [0]

    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
