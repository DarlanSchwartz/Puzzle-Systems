using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardReader : SaveableObject
{
    [Space(10)]
    [Header("States and conditions")]
    public bool hasCard = false;
    public bool sendSignalOnInput = false;
    public bool sendSignalOnRemove = false;
    public int requiredCardId;
    public int currentCardId;
    public List<ItemPlaceItem> items;
    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueCardCorrect;
    public string ParameterValueCardIncorrect;

    private void OnDrawGizmos()
    {
        
    }

    public void InputCard(int cardId)
    {
        if(!hasCard)
        {
            hasCard = true;
            EnableItem(cardId);
            currentCardId = cardId;

            if (sendSignalOnInput)
            {
                SendSignal();
            }
        }
        else
        {
            Debug.Log("Tryed to put a card while there is already a card");
        }
    }

    public void RemoveCard()
    {
        hasCard = false;
        DisableItem(currentCardId);
        currentCardId = -1;

        if (sendSignalOnRemove)
        {
            SendSignal();
        }
    }

    public void SendSignal()
    {
        if(hasCard)
        {
            if (currentCardId == requiredCardId)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCardCorrect);
            }
            else
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCardIncorrect);
            }
        }
        else
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCardIncorrect);
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

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        hasCard = bool.Parse(loadedData[1]);

        currentCardId = int.Parse(loadedData[0]);
        
        if(hasCard)
        {
            EnableItem(currentCardId);
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = currentCardId.ToString() + "|" + hasCard.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
