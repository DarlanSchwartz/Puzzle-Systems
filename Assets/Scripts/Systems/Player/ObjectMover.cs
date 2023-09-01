
using UnityEngine;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour
{
    public string _tagObjects = "Respawn";
    public int layerObjects;
    public float trowForce = 800;
    public bool isHoldingObject;
    [Space(10)]
    public Sprite closeHandTexture;
    public Sprite openHandTexture;
    public Image handDisplayImage;
    public Camera mainCamera;
    private float distanceFromCamera = 4;
    private RaycastHit hit;

    float rotXTemp;
    float rotYTemp;
    float tempDistance;
    
    [HideInInspector]public Rigidbody rbTemp;
    Vector3 rayEndPoint;
    Vector3 tempSpeed;

    private static ObjectMover _instance;

    public static ObjectMover instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectMover>();
            }

            return _instance;
        }
    }

    void Update()
    {
        rayEndPoint = transform.position + transform.forward * distanceFromCamera;

        // Raycast foward from me
        if (rbTemp == null && Physics.Raycast(transform.position, transform.forward, out hit, 7))
        {
            // Check to see if there is an object with moveable tag and is near enought
            if (Vector3.Distance(transform.position, hit.point) <= 3 && hit.transform.tag == _tagObjects || Vector3.Distance(transform.position, hit.point) <= 3 && hit.transform.gameObject.layer == layerObjects)
            {

                handDisplayImage.enabled = true;
                handDisplayImage.sprite = openHandTexture;

                // Taking the object on mouse keydown
                if (Input.GetKeyDown(KeyCode.Mouse1) && hit.rigidbody)
                {
                    if (hit.transform.GetComponent<HingeJoint>())
                    {
                        Destroy(hit.transform.GetComponent<HingeJoint>());
                    }

                    hit.rigidbody.useGravity = true;
                    distanceFromCamera = Vector3.Distance(transform.position, hit.point);
                    rbTemp = hit.transform.GetComponent<Rigidbody>();
                    rbTemp.constraints = RigidbodyConstraints.None;
                    handDisplayImage.sprite = closeHandTexture;
                    isHoldingObject = true;
                }
            }
            else
            {
                handDisplayImage.enabled = false;
            }
        }
        else
        {
            handDisplayImage.enabled = false;
        }

        // Control the object distance from the camera
        distanceFromCamera += Input.GetAxis("Mouse ScrollWheel") * 10.0f;
        distanceFromCamera = Mathf.Clamp(distanceFromCamera, 2.5f, 6);


        if (rbTemp)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1) || Vector3.Distance(transform.position, rbTemp.transform.position) > 6)
            {
                CancelMoving();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                TrowObject();
            }

            if (mainCamera)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    rotXTemp = Input.GetAxis("Mouse X") * 5.0f;
                    rotYTemp = Input.GetAxis("Mouse Y") * 5.0f;
                    rbTemp.transform.Rotate(mainCamera.transform.up, -rotXTemp, Space.World);
                    rbTemp.transform.Rotate(mainCamera.transform.right, rotYTemp, Space.World);
                }
            }
        }
    }

    private void CancelMoving()
    {
        rbTemp = null;
        handDisplayImage.sprite = openHandTexture;
        handDisplayImage.enabled = false;
        isHoldingObject = false;
    }

    void FixedUpdate()
    {
        if (rbTemp)
        {
            rbTemp.angularVelocity = new Vector3(0, 0, 0);
            tempSpeed = (rayEndPoint - rbTemp.transform.position);
            tempSpeed.Normalize();
            tempDistance = Vector3.Distance(rayEndPoint, rbTemp.transform.position);
            tempDistance = Mathf.Clamp(tempDistance, 0, 1);
            rbTemp.velocity = Vector3.Lerp(rbTemp.velocity, tempSpeed * 7.5f * tempDistance, Time.deltaTime * 12);
        }
    }

    public void TrowObject()
    {
        Vector3 tempDirection = rayEndPoint - transform.position;
        tempDirection.Normalize();
        rbTemp.useGravity = true;
        rbTemp.AddForce(tempDirection * trowForce);
        rbTemp = null;
        handDisplayImage.sprite = openHandTexture;
        handDisplayImage.enabled = false;
        isHoldingObject = false;
    }
}
