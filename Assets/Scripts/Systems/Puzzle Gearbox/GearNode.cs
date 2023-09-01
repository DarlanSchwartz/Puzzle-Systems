using UnityEngine;

public enum GearNodeType { Master , Simple}

public class GearNode : SaveableObject
{
    internal GearNodesManager manager;
    public bool hasAntecessor = true;
    public GearNode Antecessor;
    public GearNodeType gearType = GearNodeType.Simple;
    public bool hasSourceEnergy = false;
    public bool hasMovement;
    public Vector3 rotationAxis = Vector3.up;
    public float sizeFactor = 1;

    private void OnDrawGizmos()
    {
        
    }

    public void UpdateMovementState()
    {
        if(gearType == GearNodeType.Master)
        {
            if(!hasAntecessor)
            {
                hasMovement = hasSourceEnergy;
            }
            else
            {
                hasMovement = Antecessor.hasMovement;
                manager.isFullyOperating = hasMovement && manager.hasEnergy;
                manager.UpdateOperatingMaterial();
            }

            return;
        }

        if (hasAntecessor && GetComponent<Renderer>().enabled)
        {
            hasMovement = Antecessor.hasMovement;

            //print(hasMovement ? "Enabled" + name + " his antecessor movement was:" + Antecessor.hasMovement : "not enabled" + name + " his antecessor movement was:" + Antecessor.hasMovement);
        }

        
    }
    [ContextMenu("Place")]
    public void Place()
    {
        GetComponent<Renderer>().enabled = true;
        manager.UpdateAll();
    }

    [ContextMenu("Remove")]
    public void Remove()
    {
        GetComponent<Renderer>().enabled = false;
        hasMovement = false;
        manager.UpdateAll();
    }

    public void Rotate()
    {
        transform.Rotate(rotationAxis.normalized * manager.Speed(sizeFactor) * Time.deltaTime);
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
        GetComponent<Renderer>().enabled = bool.Parse(loadedData[1]);
        transform.localRotation = Quaternion.Euler(new Vector3(float.Parse(loadedData[2]), float.Parse(loadedData[3]), float.Parse(loadedData[4])));
        hasMovement = bool.Parse(loadedData[0]);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        string rotationString = transform.localRotation.eulerAngles.x + "|" + transform.localRotation.eulerAngles.y + "|" + transform.localRotation.eulerAngles.z;
        dataToSave = hasMovement.ToString() + "|" + GetComponent<Renderer>().enabled + "|" + rotationString;
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
