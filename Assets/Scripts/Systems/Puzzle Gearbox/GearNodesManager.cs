using UnityEngine;

public class GearNodesManager : SaveableObject
{
    public GearNode[] Nodes;
    public float speedRotation = 180;
    public bool hasEnergy;
    public bool isFullyOperating = false;
    private bool wasFullyOperating;
    public MeshRenderer indicatorOperating;
    public MeshRenderer indicatorInputEnergy;
    public Material onMaterial;
    public Material offMaterial;
    [Space(10)]
    [Header("Signals")]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public MessageBehavior messageMode = MessageBehavior.Always;
    public string ParameterValueFullyOperating;
    public string ParameterValueStopped;

    private void OnDrawGizmos()
    {
        
    }

    void FixedUpdate()
    {
        if(hasEnergy)
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i].hasMovement)
                {
                    Nodes[i].Rotate();
                }
            }

            CheckCompletion();
        }
    }

    public void UpdateOperatingMaterial()
    {
        indicatorOperating.material = isFullyOperating && hasEnergy ? onMaterial : offMaterial;
    }

    private void CheckCompletion()
    {
        switch (messageMode)
        {
            case MessageBehavior.OneStateChanged:
                if(isFullyOperating != wasFullyOperating)
                {
                    if (isFullyOperating)
                    {
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueFullyOperating);
                    }
                    else
                    {
                        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueStopped);
                    }

                    indicatorOperating.material = isFullyOperating ? onMaterial : offMaterial;
                }
                break;
            case MessageBehavior.Always:
                if(isFullyOperating)
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueFullyOperating);
                }
                else
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueStopped);
                }
                break;
        }

        wasFullyOperating = isFullyOperating;
    }

    public void UpdateAll()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].UpdateMovementState();
        }
    }

    public void SetEnergyState(string state)
    {
        if (state == "Energized")
        {
            if (!hasEnergy)
            {
                hasEnergy = true;
            }
        }
        else if (state == "NotEnergized")
        {
            if (hasEnergy)
            {
                hasEnergy = false;
            }
        }

        indicatorInputEnergy.material = hasEnergy ? onMaterial : offMaterial;

        UpdateOperatingMaterial();

        UpdateAll();

        if (isFullyOperating)
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueFullyOperating);
        }
        else
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueStopped);
        }
    }

    private void SetNodesManager()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].manager = this;
        }
    }

    public float Speed (float factor)
    {
        return speedRotation * factor;
    }

    [ContextMenu("Update Tags")]
    private void UpdateTags()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].transform.tag = "GearNode";
        }
    }

    [ContextMenu("Set colliders")]
    private void SetColliders()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            if(!Nodes[i].gameObject.GetComponent<MeshCollider>())
            {
                Nodes[i].gameObject.AddComponent<MeshCollider>();
                Nodes[i].gameObject.GetComponent<MeshCollider>().sharedMesh = Nodes[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
                Nodes[i].gameObject.GetComponent<MeshCollider>().convex = true;
            }
            else
            {
                Nodes[i].gameObject.GetComponent<MeshCollider>().sharedMesh = Nodes[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
                Nodes[i].gameObject.GetComponent<MeshCollider>().convex = true;
            }
        }
    }

    [ContextMenu("Update Rotations")]
    private void UpdtRotAxis()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            if(Nodes[i].Antecessor)
            {
                Nodes[i].rotationAxis = -Nodes[i].Antecessor.rotationAxis;
            }
        }
    }

    [ContextMenu("Change Energy State")]
    private void SwitchEnergy()
    {
        SetEnergyState(hasEnergy ? "NotEnergized" : "Energized");
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();

        SetNodesManager();
        UpdateAll();
        UpdateOperatingMaterial();
        indicatorInputEnergy.material = hasEnergy ? onMaterial : offMaterial;
        Nodes[0].hasSourceEnergy = true;
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        hasEnergy = bool.Parse(loadedData[0]);
        isFullyOperating = bool.Parse(loadedData[1]);
        wasFullyOperating = bool.Parse(loadedData[2]);

        indicatorInputEnergy.material = hasEnergy ? onMaterial : offMaterial;

        UpdateOperatingMaterial();
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = hasEnergy.ToString() + "|" + isFullyOperating.ToString() + "|" + wasFullyOperating.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }


}
