using UnityEngine;

public class ItemNeeder : SaveableObject
{
    [Space(10)]
    [Header("Parameters and values")]
    public ItemNeederItemPlace[] neededItems;
    [Tooltip("Setting this to true if you remove an item from one slot after you have completed the puzzle it will cancel the completion.")]
    public bool canCancelCompletion = true;
    private bool completed = false;
    [Space(10)]
    [Header("Signal")]
    [Tooltip("Setting this to true if you remove an item from one slot after you have completed the puzzle it will send to the receiver a message.")]
    public bool sendMessageOnRemove = true;
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnComplete;
    public string ParameterValueOnRemoveItemAfterComplete;

    public void SetItem(string indexAndId)
    {
        string[] values = indexAndId.Split('_');

        int index = int.Parse(values[0]);
        int id = int.Parse(values[1]);

        neededItems[index].current = id;

        if(id == -1 && completed && sendMessageOnRemove)
        {
            if(canCancelCompletion)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnRemoveItemAfterComplete);
                completed = false;
            }
            else
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnRemoveItemAfterComplete);
            }
        }

        for (int i = 0; i < neededItems.Length; i++)
        {
            if (neededItems[i].current != neededItems[i].needed)
            {
                return;
            }
        }

        PuzzleComplete();
    }

    private void PuzzleComplete()
    {
        if(!completed)
        {
            completed = true;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnComplete);
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

        for (int i = 0; i < neededItems.Length; i++)
        {
            neededItems[i].current = int.Parse(loadedData[i]);
        }

        completed = bool.Parse(loadedData[2]);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = this.ToString();
    }

    public override string ToString()
    {
        string data = string.Empty;

        for (int i = 0; i < neededItems.Length; i++)
        {
            if(i < neededItems.Length -1)
            {
                data += neededItems[i].current + "|";
            }
            else
            {
                data += neededItems[i].current;
            }
        }

        data += "|" + completed.ToString();

        return data;
    }

    private void OnDrawGizmos()
    {
        
    }
}
