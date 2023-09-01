using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public List<Slot> slots;
    public bool isOpen;
    public Transform inventoryHolder;
    public FirstPersonController controller;
    public Slot movingItem;
    public Transform dynamicObjectsParent;
    public Transform dropPosition;
    public bool isMovingItem;

    private static Inventory _instance;

    public static Inventory instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Inventory>();
            }

            return _instance;
        }
    }

    public void StartMovingItem(Slot from)
    {
        if(!isMovingItem && from.currentItem !=null)
        {
            movingItem.gameObject.SetActive(true);
            movingItem.SetItem(from.currentItem);
            from.Clear();
            isMovingItem = true;
        }
    }

    public void PlaceItem(Slot where)
    {
        if(isMovingItem)
        {
            // If the slot clicked is empty just clear moving item and place it on the clicked slot, else swap between the 2
            if (where.isEmpty())
            {
                where.SetItem(movingItem.currentItem);
                int placedItemId = movingItem.currentItem.itemId;
                StopMovingItem();
                Tooltip.instance.EnableTooltip(placedItemId);
            }
            else
            {
                Item temp = where.currentItem;
                where.SetItem(movingItem.currentItem);
                movingItem.SetItem(temp);
            }
        }
    }

    private void Update()
    {
        if(isMovingItem)
        {
            movingItem.transform.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);

            Cursor.visible = false;

            //Delete the moving item
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/" + movingItem.currentItem.prefabName), dropPosition.position, Quaternion.identity, dynamicObjectsParent);
                StopMovingItem();
            }
        }
    }

    public void StopMovingItem()
    {
        movingItem.Clear();
        movingItem.gameObject.SetActive(false);
        isMovingItem = false;
        Cursor.visible = true;
    }

    public void CancelMovingItem()
    {
        if(isMovingItem)
        {
            AddItem(movingItem.currentItem.itemId);
            movingItem.Clear();
            movingItem.gameObject.SetActive(false);
            isMovingItem = false;
            Cursor.visible = true;
        }
    }

    private void Start()
    {
        foreach (Slot slot in slots)
        {
            slot.Clear();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }

    public void AddItem(int id)
    {
        foreach(Slot slot in slots)
        {
            if(slot.isEmpty())
            {
                slot.SetItem(InventoryDatabase.instance.GetItemWithID(id));
                break;
            }
        }
    }

    public void Open()
    {
       if(!isOpen && !ManagerGameplay.instance.isOpen)
        {
            isOpen = true;
            inventoryHolder.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            controller.m_MouseLook.XSensitivity = 0;
            controller.m_MouseLook.YSensitivity = 0;
        }
    }

    public void Close()
    {
       if(isOpen)
        {
            isOpen = false;
            Tooltip.instance.DisableTooltip();
            inventoryHolder.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            controller.m_MouseLook.XSensitivity = 2;
            controller.m_MouseLook.YSensitivity = 2;
            CancelMovingItem();
        }
    }

    public void Interact()
    {
        if(isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public bool HasItem(int id)
    {
        foreach(Slot slot in slots)
        {
            if(!slot.isEmpty())
            {
                if (slot.currentItem.itemId == id)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveItem(int id)
    {
        if(HasItem(id))
        {
            foreach (Slot slot in slots)
            {
                if (slot.currentItem !=null && slot.currentItem.itemId == id)
                {
                    slot.Clear();
                    break;
                }
            }
        }
    }

    public int RemoveItem(ItemType type)
    {
        int cardId = -1;

        if (HasItemType(type))
        {
            foreach (Slot slot in slots)
            {
                if (slot.currentItem != null && slot.currentItem.type == type)
                {
                    cardId = slot.currentItem.itemId;
                    slot.Clear();
                    break;
                }
            }
        }

        return cardId;
    }

    public bool isFull()
    {
        foreach(Slot slot in slots)
        {
            if(slot.isEmpty())
            {
                return false;
            }
        }

        return true;
    }

    public bool HasItemType(ItemType itemType)
    {
        foreach (Slot slot in slots)
        {
            if (!slot.isEmpty())
            {
                if (slot.currentItem.type == itemType)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public Item GetFirstItem()
    {
        foreach(Slot slot in slots)
        {
            if(!slot.isEmpty())
            {
                return slot.currentItem;
            }
        }

        return null;
    }

    public bool isEmpty()
    {
        foreach (Slot slot in slots)
        {
            if (!slot.isEmpty())
            {
                return false;
            }
        }

        return true;
    }
}
