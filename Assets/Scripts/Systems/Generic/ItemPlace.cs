using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPlace : SaveableObject
{
    public bool hasItem;
    public List<ItemPlaceItem> items;
    public int currentItemId = -1;

    [Space(10)]
    [Header("Signals")]
    public YesOrNo useReceiver = YesOrNo.yes;
    
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public ParameterMode parameterMode = ParameterMode.OnlyParameter;
    public bool sendIdOnRemove = false;
    public string ParameterValueOnPlace;
    public string ParameterValueOnRemove;

    [Space(10)]
    [Header("Sounds")]
    public AudioSource mainSource;
    public AudioClip onPlaceSound;
    public AudioClip onRemoveSound;

    public bool Disabled { get; set; } = false;

    private void OnDrawGizmos()
    {
        
    }

    public void DisableItem(int id)
    {
        foreach (ItemPlaceItem item in items)
        {
            if (item.itemId == id)
            {
                item.itemMesh.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void EnableItem(int id)
    {
        foreach (ItemPlaceItem item in items)
        {
            if (item.itemId == id)
            {
                item.itemMesh.gameObject.SetActive(true);
                break;
            }
        }
    }

    public bool CanPlaceItem(int itemId)
    {
        foreach(ItemPlaceItem itemFilterId in items)
        {
            if(itemFilterId.itemId == itemId)
            {
                return true;
            }
        }

        return false;
    }

    public void PlaceItem(int itemId)
    {
        if(!hasItem && CanPlaceItem(itemId))
        {
            hasItem = true;
            EnableItem(itemId);
            currentItemId = itemId;

            if (mainSource !=null && onPlaceSound !=null)
            {
                mainSource.PlayOneShot(onPlaceSound);
            }

            if(receiver !=null)
            {
                switch (parameterMode)
                {
                    case ParameterMode.OnlyParameter:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnPlace);
                        break;
                    case ParameterMode.ParameterPlusId:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnPlace + itemId);
                        break;
                    case ParameterMode.OnlyId:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(),itemId);
                        break;
                    case ParameterMode.IdPlusParameter:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), itemId + ParameterValueOnPlace);
                        break;
                }
            }
            else
            {
                if(useReceiver == YesOrNo.yes)
                {
                    Debug.LogError("No receiver");
                }
            }
        }

        if(!CanPlaceItem(itemId))
        {
            Debug.LogWarning("Cant place item : '" + itemId + "'");
        }
    }

    public void RemoveItem()
    {
        if(hasItem)
        {
            if (mainSource != null && onRemoveSound != null)
            {
                mainSource.PlayOneShot(onRemoveSound);
            }

            hasItem = false;
            DisableItem(currentItemId);
            currentItemId = -1;

            if (receiver !=null)
            {
                switch (parameterMode)
                {
                    case ParameterMode.OnlyParameter:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnRemove);
                        break;
                    case ParameterMode.ParameterPlusId:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnRemove + (sendIdOnRemove ? currentItemId : -1));
                        break;
                    case ParameterMode.OnlyId:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), (sendIdOnRemove ? currentItemId : -1));
                        break;
                    case ParameterMode.IdPlusParameter:
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), (sendIdOnRemove ? currentItemId : -1) + ParameterValueOnRemove);
                        break;
                }
            }
            else
            {
                if (useReceiver == YesOrNo.yes)
                {
                    Debug.LogError("No receiver");
                }
            }
        }
    }

    public MeshRenderer GetCurrentItemMesh
    {
        get
        {
            if (hasItem && currentItemId != -1)
            {
                foreach (ItemPlaceItem item in items)
                {
                    if (item.itemId == currentItemId)
                    {
                        return item.itemMesh.GetComponent<MeshRenderer>();
                    }
                }
            }

            return null;
        }
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        foreach (ItemPlaceItem item in items)
        {
            if (item.itemMesh.gameObject.activeSelf)
            {
                hasItem = true;
                break;
            }
        }
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        hasItem = bool.Parse(loadedData[0]);
        currentItemId = int.Parse(loadedData[1]);
        if(hasItem)
        {
            EnableItem(currentItemId);
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = hasItem.ToString() + "|" + currentItemId;
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
