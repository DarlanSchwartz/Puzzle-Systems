using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveableObject : MonoBehaviour
{

    protected string dataToSave;
    [Space(10)]
    [Header("Save information")]
    [Tooltip("This value controls if this object will be added to the dynamic save objects list or not, dynamic objects get positions saved, statics dont.")]
    public bool isDynamic = true;
    [SerializeField]
    [Tooltip("This value to instantiate the prefab of this game object when loaded, this prefab must have the same name as here and be located on the Resources folder.")]
    private string prefabName;

    private void OnDrawGizmos()
    {
        
    }

    public virtual void Start()
    {
        if(isDynamic)
        {
            ManagerSave.Instance.dynamicObjectsToSave.Add(this);
        }
    }

    public string GetCurrentData
    {
        get
        {
            return dataToSave;
        }
    }

    public virtual void SetCurrentData(string dataInfo)
    {
        dataToSave = dataInfo;
    }

    public virtual void UpdateDataToSaveToCurrentData()
    {
        if(isDynamic)
        {
            string positionString = transform.position.x + "|" + transform.position.y + "|" + transform.position.z + "|";
            string rotationString = transform.rotation.eulerAngles.x + "|" + transform.rotation.eulerAngles.y + "|" + transform.rotation.eulerAngles.z;
            dataToSave = prefabName + "|" + positionString + rotationString;
        }
    }

    public virtual void LoadFromCurrentData()
    {
        if(isDynamic)
        {
            string[] values = dataToSave.Split('|');

            transform.position = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
            transform.rotation = Quaternion.Euler(float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]));
        }
    }

    public virtual void DestroySaveable()
    {
        if(isDynamic)
        {
            ManagerSave.Instance.dynamicObjectsToSave.Remove(this);
        }
        else
        {
            ManagerSave.Instance.staticObjectsToSave.Remove(this);
        }

        Destroy(this);
    }
}

