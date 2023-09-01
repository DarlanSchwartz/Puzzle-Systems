using UnityEngine;
using System.Collections.Generic;

public class ItemInspector : MonoBehaviour
{
    private WorldItem currentItem;
    public WorldItem currentLookingItem;
    public List<Transform> lookingItems;
    public float sensitivity;
    private Vector2 lastMousePosition = Vector2.zero;


    private void Start()
    {
        SetCurrentLookingItem(0,lookingItems[0].GetComponent<WorldItem>());
    }

    void Update()
    {
        Vector2 currentMousePosition = (Vector2)Input.mousePosition;
        Vector2 mouseDelta = currentMousePosition - lastMousePosition;
        mouseDelta *= sensitivity * Time.deltaTime;

        lastMousePosition = currentMousePosition;

        if(Input.GetMouseButton(0))
        {
            currentLookingItem.transform.Rotate(mouseDelta.y * -1f, mouseDelta.x * -1f, 0f, Space.World);
            Vector3 eulerRot = currentLookingItem.transform.rotation.eulerAngles;
            eulerRot.z = 0;
            currentLookingItem.transform.rotation = Quaternion.Euler(eulerRot);
        }
        
        //if(currentLookingItem)
        //{
        //    currentLookingItem.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime));

        //    if(Input.GetMouseButtonUp(0))
        //    {
        //        //Add to inventory
        //        print("Added to inventory" + currentLookingItem.Id);
        //        StopInspecting();
        //    }
        //    else if (Input.GetMouseButtonUp(1))
        //    {
        //        StopInspecting();
        //    }
        //}
    }

    public void SetCurrentLookingItem(int id, WorldItem worldItem)
    {
        foreach (Transform item in lookingItems)
        {
            if(item.GetComponent<WorldItem>().Id == id)
            {
                currentLookingItem = item.GetComponent<WorldItem>();
                item.gameObject.SetActive(true);
                break;
            }
        }

        currentItem = worldItem;

        //worldItem.gameObject.SetActive(false);

        //Show the hud
        //Put the info of the item in the hud
    }

    public void StopInspecting()
    {
        if(currentLookingItem)
        {
            currentLookingItem.gameObject.SetActive(false);
            currentLookingItem.transform.rotation = Quaternion.identity;
            currentLookingItem = null;
        }
    }
}
