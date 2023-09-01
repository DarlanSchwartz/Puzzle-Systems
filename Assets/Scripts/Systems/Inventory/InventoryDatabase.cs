using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDatabase : MonoBehaviour
{
    public List<Item> items;

    private static InventoryDatabase _instance;

    public static InventoryDatabase instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryDatabase>();
            }

            return _instance;
        }
    }

    public Item GetItemWithID(int itemID)
    {
        foreach(Item item in items)
        {
            if(item.itemId == itemID)
            {
                return item;
            }
        }

        Debug.LogError("Item id: " + itemID + " not found in the database!");

        return null;
    }

    public string GetItemName(int itemID)
    {
        foreach (Item item in items)
        {
            if (item.itemId == itemID)
            {
                return item.itemName;
            }
        }

        Debug.LogError("Item id: " + itemID + " not found in the database!");

        return string.Empty;
    }

    public void RemoveItem(int itemID)
    {
        foreach(Item item in items)
        {
            if(item.itemId == itemID)
            {
                items.Remove(item);
            }
        }
    }

    public bool HasItem(Item item)
    {
        foreach(Item itemLoop in items)
        {
            if(itemLoop == item)
            {
                return true;
            }
        }

        return false;
    }

    public void AddItem(int itemID)
    {

    }
}
