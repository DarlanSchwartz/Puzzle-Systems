using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerMechanics : MonoBehaviour
{
    public Camera mainCamera;
    public bool changeFOV = false;
    public FOVChanger FovChanger;
    public InfoShower info;
    public bool DebugRay;

    private void Awake()
    {
        if (QualitySettings.GetQualityLevel() == 1)
        {
            FindObjectOfType<PostProcessVolume>().weight = 0;
        }
    }

    void Update()
    {
        if(ManagerGameplay.instance.isOpen)
        {
            info.textInfo.text = string.Empty;
        }
        else
        {
            info.UpdateInfo(mainCamera);
        }

        if (changeFOV)
        {
            FovChanger.FOVChange(mainCamera);
        }

        if(Input.GetKeyDown(KeyCode.Tab) && !ManagerGameplay.instance.isOpen)
        {
            Inventory.instance.Interact();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Inventory.instance.isOpen)
            {
                Inventory.instance.Close();
            }
            else
            {
                if(!ManagerGameplay.instance.isOpen)
                {
                    ManagerGameplay.instance.Open();
                }
                else
                {
                    if (ManagerGameplay.instance.SavegameWindowObject.gameObject.activeSelf)
                    {
                        ManagerGameplay.instance.CloseSaveGameScreen();
                    }
                    else if (ManagerGameplay.instance.SavegameNameObject.gameObject.activeSelf)
                    {
                        ManagerGameplay.instance.CloseSaveGameNameScreen();
                    }
                    else if (ManagerGameplay.instance.LoadGameMenuObject.gameObject.activeSelf)
                    {
                        ManagerGameplay.instance.CloseLoadGameScreen();
                    }
                    else if (ManagerGameplay.instance.AreYouSureObject.gameObject.activeSelf)
                    {
                        ManagerGameplay.instance.CancelQuit();
                    }
                    else if(ManagerGameplay.instance.menuObject.gameObject.activeSelf)
                    {
                        ManagerGameplay.instance.Close();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!DebugRay)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(mainCamera.transform.position, info.hit.point);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(info.hit.point, 0.01f);
    }
}

[System.Serializable]
public class FOVChanger
{
    public float defaultValue = 60;
    public float targetValue = 45;
    [Range(0,1)]
    public float smoothness = 0.1f;

    // Change fov between default and target values depending on mouse1 is pressed
    public void FOVChange(Camera cam)
    {
        cam.fieldOfView = Input.GetMouseButton(1) ? Mathf.Lerp(cam.fieldOfView, targetValue, smoothness) : Mathf.Lerp(cam.fieldOfView, defaultValue, smoothness);
    }
}

[System.Serializable]
public class InfoShower
{
    public RaycastHit hit;
    public ObjectMover objectMover;
    public Text textInfo;
    private MeshRenderer tempItem;
    private Shader tempShader;
    private Color tempColor;
    public Shader outlineShader;
    public void UpdateInfo(Camera cam)
    {
        if (Inventory.instance.isOpen)
        {
            SetInfo(string.Empty);
            return;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(hit.transform.name + " " + hit.transform.tag);
        }

        UpdateInfoShower(cam);
    }

    private void LMBClicked()
    {
        if (hit.transform.tag == "Button")
        {
            hit.transform.SendMessage("Press");
        }
        else if (hit.transform.tag == "Door")
        {
            Openable tempDoor = hit.transform.GetComponent<Openable>();

            if (!tempDoor.requireKey || tempDoor.requireKey && !tempDoor.isLocked || tempDoor.requireKey && tempDoor.canBeOpenedWithKey && !Inventory.instance.HasItem(tempDoor.keyId))
            {
                hit.transform.SendMessage("Interact");
            }
        }
        else if (hit.transform.tag == "LaserBox")
        {
            hit.transform.SendMessage("Press");
        }
        else if (hit.transform.tag == "Switch")
        {
            hit.transform.SendMessage("Pull");
        }
        else if (hit.transform.tag == "SwitchChild")
        {
            hit.transform.parent.SendMessage("Pull");
        }
        else if (hit.transform.tag == "GravityHolder")
        {
            hit.transform.SendMessage("ChangeGravityState");
        }
        else if (hit.transform.tag == "KeycardReader")
        {
            hit.transform.SendMessage("SendSignal");
        }
        else if (hit.transform.tag == "EnergyNodeLoki")
        {
            hit.transform.SendMessage("InteractAction");
        }
        else if (hit.transform.tag == "SliderPuzzlePiece")
        {
            hit.transform.SendMessage("Move");
        }
    }

    private void UpdateInfoShower(Camera cam)
    {
        // Show info about the objects in the center of view depending on tag
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3))
        {
            if (hit.transform.tag == "Button")
            {
                SetInfo("Press LMB to push.");
            }
            else if (hit.transform.tag == "Door")
            {
                Openable tempDoor = hit.transform.GetComponent<Openable>();

                SetOutlineOn(tempDoor.transform.GetComponent<MeshRenderer>());

                if(!tempDoor.IsMoving)
                {
                    if (tempDoor.isLocked)
                    {
                        if (tempDoor.requireKey)
                        {
                            if (tempDoor.canBeOpenedWithKey && Inventory.instance.HasItem(tempDoor.keyId))
                            {
                                SetInfo("Press F to unlock.");
                            }
                            else
                            {
                                SetInfo("Is locked.");
                            }
                        }
                        else
                        {
                            SetInfo("Is locked.");
                        }
                    }
                    else
                    {
                        if (tempDoor.isOpen)
                        {
                            SetInfo("Press LMB to close.");
                        }
                        else
                        {
                            SetInfo("Press LMB to open.");
                        }
                    }
                }
                else
                {
                    SetInfo(string.Empty);
                }
                
            }
            else if (hit.transform.tag == "LaserBox")
            {
                SetInfo("Press LMB to Rotate.");
            }
            else if (hit.transform.tag == "Switch" || hit.transform.tag == "SwitchChild")
            {
                SetInfo("Press LMB to Pull.");

                SetOutlineOn(hit.transform.GetComponent<MeshRenderer>());
            }
            else if (hit.transform.tag == "GravityHolder")
            {
                SetInfo("Press LMB to cut.");
            }
            else if (hit.transform.tag == "Item")
            {
                if (ObjectMover.instance.isHoldingObject && ObjectMover.instance.rbTemp.gameObject != hit.transform.gameObject || !ObjectMover.instance.isHoldingObject)
                {
                    if (hit.transform.gameObject.layer == ObjectMover.instance.layerObjects)
                    {
                        if (!Inventory.instance.isFull())
                        {
                            SetInfo("Press 'E' to pickup or hold RMB to grab.");
                        }
                        else
                        {
                            SetInfo("Hold RMB to grab.");
                        }
                    }
                    else
                    {
                        SetInfo("Press 'E' to pickup.");
                    }

                    SetOutlineOn(hit.transform.GetComponent<MeshRenderer>());
                }
            }
            else if (hit.transform.tag == "ItemPlace")
            {
                ItemPlace tempPlace = hit.transform.GetComponent<ItemPlace>();

                if (!tempPlace.hasItem && !Inventory.instance.isEmpty())
                {
                    if (tempPlace.CanPlaceItem(Inventory.instance.GetFirstItem().itemId))
                    {
                        SetInfo("Press 'F' to place a " + Inventory.instance.GetFirstItem().itemName + ".");
                    }
                    else
                    {
                        SetInfo("You can't place a " + Inventory.instance.GetFirstItem().itemName + " in this slot.");
                    }
                }
                else if (tempPlace.hasItem)
                {
                    if (!Inventory.instance.isFull())
                    {
                        SetInfo("Press 'E' to pickup '" + InventoryDatabase.instance.GetItemName(tempPlace.currentItemId) + "'.");
                    }
                    else
                    {
                        SetInfo("Inventory Full");
                    }

                    SetOutlineOn(tempPlace.GetCurrentItemMesh);
                }
            }
            else if (hit.transform.tag == "ItemPlaceRotator")
            {
                ItemPlace tempPlace = hit.transform.GetComponent<ItemPlace>();

                if (!tempPlace.Disabled)
                {
                    if (!tempPlace.hasItem && !Inventory.instance.isEmpty())
                    {
                        if (tempPlace.CanPlaceItem(Inventory.instance.GetFirstItem().itemId))
                        {
                            SetInfo("Press 'F' to place a " + Inventory.instance.GetFirstItem().itemName + ".");
                        }
                        else
                        {
                            SetInfo("You can't place a " + Inventory.instance.GetFirstItem().itemName + " in this slot.");
                        }
                    }
                    else if (tempPlace.hasItem)
                    {
                        if (!Inventory.instance.isFull())
                        {
                            SetInfo("Press 'E' to pickup '" + InventoryDatabase.instance.GetItemName(tempPlace.currentItemId) + " Press 'Q or 'R' to rotate.");
                        }
                        else
                        {
                            SetInfo("Inventory Full / Press 'Q or 'R' to rotate.");
                        }

                        SetOutlineOn(tempPlace.GetCurrentItemMesh);
                    }
                }
            }
            else if (hit.transform.tag == "Dynamic")
            {
                if (objectMover.rbTemp != null)
                {
                    SetInfo("Release RMB to release or LMB to trow.");
                }
                else
                {
                    SetInfo("Hold RMB to grab.");
                }
            }
            else if (hit.transform.tag == "KeycardReader")
            {
                KeycardReader reader = hit.transform.GetComponent<KeycardReader>();

                if (!reader.hasCard && Inventory.instance.HasItemType(ItemType.Keycard))
                {
                    SetInfo("Press 'F' to place a card.");
                }
                else if (reader.hasCard)
                {
                    SetInfo("Press 'F' to remove the card or LMB to try operation.");
                }
                else if (!reader.hasCard && !Inventory.instance.HasItemType(ItemType.Keycard))
                {
                    SetInfo("You don't have a keycard on your inventory!");
                }
            }
            else if (hit.transform.tag == "EnergyNode")
            {
                SetInfo("Press 'Q' and 'E' to rotate.");

                SetOutlineOn(hit.transform.GetChild(0).GetComponent<MeshRenderer>());
            }
            else if (hit.transform.tag == "EnergyNodeLoki")
            {
                EnergyNodeLoki tempLoki = hit.transform.GetComponent<EnergyNodeLoki>();

                if(!tempLoki.hasEnergy)
                {
                    SetInfo("Press LMB to interact.");
                }
                else
                {
                    SetInfo(string.Empty);
                }
            }
            else if (hit.transform.tag == "SliderPuzzlePiece")
            {
                SliderPuzzlePiece tempPiece = hit.transform.GetComponent<SliderPuzzlePiece>();

                if (tempPiece.canInteract && tempPiece.canMove)
                {
                    SetInfo("Press LMB to interact.");
                }
                else
                {
                    SetInfo(string.Empty);
                }
            }
            else if (hit.transform.tag == "GearNode")
            {
                GearNode gearNode = hit.transform.GetComponent<GearNode>();

                int id = gearNode.sizeFactor == 1 ? 7 : 8;

                if (!hit.transform.GetComponent<MeshRenderer>().enabled && !Inventory.instance.isEmpty())
                {
                    if (Inventory.instance.GetFirstItem().itemId == id)
                    {
                        SetInfo("Press 'F' to place a " + Inventory.instance.GetFirstItem().itemName + ".");

                        SetOutlineOn(hit.transform.GetComponent<MeshRenderer>());
                    }
                    else
                    {
                        SetInfo("You can't place a " + Inventory.instance.GetFirstItem().itemName + " in this slot.");
                    }
                }
                else if (hit.transform.GetComponent<MeshRenderer>().enabled && !Inventory.instance.isFull())
                {
                    SetInfo("Press 'E' to pickup '" + InventoryDatabase.instance.GetItemName(id) + "'.");

                    SetOutlineOn(hit.transform.GetComponent<MeshRenderer>());
                }
                else
                {
                    SetInfo(string.Empty);
                    ClearTempItem();
                }
            }
            else
            {
                SetInfo(string.Empty);

                ClearTempItem();
            }

            if (Input.GetMouseButtonDown(0))
            {
                LMBClicked();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                PressedKeyE();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                PressedKeyF();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                PressedKeyQ();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                PressedKeyR();
            }
        }
        else
        {
            if (objectMover.rbTemp != null)
            {
                SetInfo("Release RMB to release or LMB to trow.");
            }
            else
            {
                SetInfo(string.Empty);
            }

            ClearTempItem();
        }
    }

    private void SetOutlineOn( MeshRenderer outlineTarget)
    {
        if (tempItem)
        {
            if (outlineTarget != tempItem)
            {
                ClearTempItem();
                tempShader = outlineTarget.material.shader;
                tempItem = outlineTarget;
                tempColor = outlineTarget.material.GetColor("_Color");
                outlineTarget.material.shader = outlineShader;
                outlineTarget.material.SetColor("_Color", tempColor);
                outlineTarget.material.SetColor("_OutlineColor", Color.white);
                outlineTarget.material.renderQueue = 3000;
            }
        }
        else
        {
            tempShader = outlineTarget.material.shader;
            tempItem = outlineTarget;
            tempColor = outlineTarget.material.GetColor("_Color");
            outlineTarget.material.shader = outlineShader;
            outlineTarget.material.SetColor("_Color", tempColor);
            outlineTarget.material.SetColor("_OutlineColor", Color.white);
            outlineTarget.material.renderQueue = 3000;
        }
    }

    private void ClearTempItem()
    {
        if (tempItem)
        {
            tempItem.transform.GetComponent<MeshRenderer>().material.shader = tempShader;
            tempItem.transform.GetComponent<MeshRenderer>().material.SetColor("_Color",tempColor);
            tempItem = null;
            tempShader = null;
            tempColor = Color.white;
        }
    }

    private void PressedKeyR()
    {
        if (hit.transform.tag == "ItemPlaceRotator")
        {
            hit.transform.SendMessage("RotateMethod", true);
        }
    }

    private void PressedKeyQ()
    {
        if (hit.transform.tag == "EnergyNode" )
        {
            hit.transform.GetChild(0).SendMessage("Rotate", false);
        }
        else if (hit.transform.tag == "ItemPlaceRotator")
        {
            hit.transform.SendMessage("RotateMethod", false);
        }
    }

    private void PressedKeyF()
    {
        if (hit.transform.tag == "ItemPlace")
        {
            ItemPlace tempPlace = hit.transform.GetComponent<ItemPlace>();

            if (!tempPlace.hasItem && !Inventory.instance.isEmpty() && tempPlace.CanPlaceItem(Inventory.instance.GetFirstItem().itemId))
            {
                tempPlace.PlaceItem(Inventory.instance.GetFirstItem().itemId);
                Inventory.instance.RemoveItem(Inventory.instance.GetFirstItem().itemId);
            }
        }
        if (hit.transform.tag == "ItemPlaceRotator")
        {
            ItemPlace tempPlace = hit.transform.GetComponent<ItemPlace>();

            if(!tempPlace.Disabled)
            {
                if (!tempPlace.hasItem && !Inventory.instance.isEmpty() && tempPlace.CanPlaceItem(Inventory.instance.GetFirstItem().itemId))
                {
                    tempPlace.PlaceItem(Inventory.instance.GetFirstItem().itemId);
                    Inventory.instance.RemoveItem(Inventory.instance.GetFirstItem().itemId);
                }
            }
        }
        else if (hit.transform.tag == "Door")
        {
            Openable tempDoor = hit.transform.GetComponent<Openable>();

            if (tempDoor.isLocked && tempDoor.requireKey)
            {
                if (tempDoor.canBeOpenedWithKey && Inventory.instance.HasItem(tempDoor.keyId))
                {
                    hit.transform.SendMessage("Interact");
                }
            }
        }
        else if (hit.transform.tag == "KeycardReader")
        {
            KeycardReader reader = hit.transform.GetComponent<KeycardReader>();

            if (!reader.hasCard && Inventory.instance.HasItemType(ItemType.Keycard))
            {
                reader.InputCard(Inventory.instance.RemoveItem(ItemType.Keycard));
            }
            else if (reader.hasCard)
            {
                Inventory.instance.AddItem(reader.currentCardId);
                reader.RemoveCard();
            }
        }
        else if (hit.transform.tag == "GearNode")
        {
            GearNode gearNode = hit.transform.GetComponent<GearNode>();

            int id = gearNode.sizeFactor == 1 ? 7 : 8;

            if (!hit.transform.GetComponent<MeshRenderer>().enabled && !Inventory.instance.isEmpty() && Inventory.instance.GetFirstItem().itemId == id)
            {
                Inventory.instance.RemoveItem(Inventory.instance.GetFirstItem().itemId);
                gearNode.Place();
            }
        }
    }

    private void PressedKeyE()
    {
        if (hit.transform.tag == "Item" && !Inventory.instance.isFull())
        {
            if (ObjectMover.instance.isHoldingObject && ObjectMover.instance.rbTemp.gameObject != hit.transform.gameObject || !ObjectMover.instance.isHoldingObject)
            {
                Inventory.instance.AddItem(hit.transform.GetComponent<WorldItem>().Id);
                hit.transform.GetComponent<WorldItem>().DestroySaveable();
            }
        }
        else if (hit.transform.tag == "ItemPlace")
        {
            ItemPlace tempPlace = hit.transform.GetComponent<ItemPlace>();

            if (tempPlace.hasItem && !Inventory.instance.isFull())
            {
                Inventory.instance.AddItem(tempPlace.currentItemId);
                tempPlace.RemoveItem();
            }
        }
        else if (hit.transform.tag == "ItemPlaceRotator")
        {
            ItemPlace tempPlace = hit.transform.GetComponent<ItemPlace>();

            if(!tempPlace.Disabled)
            {
                if (tempPlace.hasItem && !Inventory.instance.isFull())
                {
                    Inventory.instance.AddItem(tempPlace.currentItemId);
                    tempPlace.RemoveItem();
                }
            }
        }
        else if (hit.transform.tag == "EnergyNode")
        {
            hit.transform.GetChild(0).SendMessage("Rotate", true);
        }
        else if (hit.transform.tag == "GearNode")
        {
            GearNode gearNode = hit.transform.GetComponent<GearNode>();

            int id = gearNode.sizeFactor == 1 ? 7 : 8;

            if (hit.transform.GetComponent<MeshRenderer>().enabled && !Inventory.instance.isFull())
            {
                Inventory.instance.AddItem(id);
                gearNode.Remove();
            }
        }
    }

    private void SetInfo(string info)
    {
        textInfo.text = info;
    }
}
