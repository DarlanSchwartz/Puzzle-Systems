using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyNodeLoki : SaveableObject
{
    public bool hasEnergy;
    public LokiManager manager;
    [Space(10)]
    [Header("Indicator")]
    public Material onMat;
    public Material offMat;
    public Renderer indicator;
    public int indexMaterial;
    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public List<Transform> receivers;
    [Space(10)]
    [Header("Sounds")]
    [Space(10)]
    public AudioSource mainSource;
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;

    [ContextMenu("Click")]
    public void InteractAction()
    {
        if (manager.isCompleted)
            return;

        if (!hasEnergy)
        {
            if (receivers.Count > 0)
            {
                foreach (Transform receiver in receivers)
                {
                    Messager.RunVoid(receiver, "ChangeCurrentState", "VoidRun", string.Empty);
                }
            }
            else
            {
                Debug.LogError("No receivers on the :" + name + " loki energy node.");
            }

            ChangeCurrentState();

            if(mainSource && turnOnSound)
            {
                mainSource.PlayOneShot(turnOnSound);
            }
        }
    }

    public void ChangeCurrentState()
    {
        if(hasEnergy)
        {
            SetEnergyOff();
        }
        else
        {
            SetEnergyOn();
        }
    }

    public void SetEnergyOn()
    {
        hasEnergy = true;

        UpdateIndicator();

        if(mainSource && turnOnSound)
        {
            mainSource.PlayOneShot(turnOnSound);
        }

        manager.CheckCompletion();
    }

    void UpdateIndicator()
    {
        if (indicator && onMat && offMat)
        {
            if (indicator.materials.Length == 1)
            {
                indicator.material = hasEnergy ? onMat : offMat;
            }
            else
            {
                Material[] mats = indicator.materials;
                mats[indexMaterial] = hasEnergy ? onMat : offMat;
                indicator.materials = mats;
            }
        }
    }

    public void SetEnergyOff()
    {
        hasEnergy = false;

        if (mainSource && turnOffSound)
        {
            mainSource.PlayOneShot(turnOffSound);
        }

        UpdateIndicator();
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        // Do this class code logic
    }

    private void OnDrawGizmos()
    {

    }

    public override void LoadFromCurrentData()
    {
        hasEnergy = bool.Parse(dataToSave);

        UpdateIndicator();
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = hasEnergy.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
