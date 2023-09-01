using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : SaveableObject
{
    public Transform endPoint;
    public float speed;
    public MeshRenderer beltRenderer;
    public int beltIndexMaterial;
    private Material beltMaterial;
    private float offset;
    public float factor;

    [Header("Acceleration and deacceleration")]
    [Range(0 , 1)]public float decreaseFactor = 0.22f;
    [Range(0 , 1)]public float increaseFactor = 1f;

    private float movementFactor;
    public bool hasEnergy = true;
    public bool isEnabled = true;
    public AudioSource mainSource;
    public AudioSource secondarySource;
    public AudioClip startSound;
    public AudioClip stopSound;

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();

        if (hasEnergy && isEnabled)
        {
            mainSource.Play();
        }

        Material[] mats = beltRenderer.materials;

        beltMaterial = mats[beltIndexMaterial];

        beltRenderer.materials = mats;
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        hasEnergy = bool.Parse(loadedData[0]);
        isEnabled = bool.Parse(loadedData[1]);
        offset = float.Parse(loadedData[2]);
        offset -=  offset > 1 ? (int)offset : 0;

        beltMaterial.SetTextureOffset("_MainTex", new Vector2(0, offset));

        if (hasEnergy && isEnabled)
        {
            mainSource.Play();
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = hasEnergy.ToString() + "|" + isEnabled.ToString() + "|"  + offset;
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }

    

    void FixedUpdate()
    {
        if(hasEnergy && isEnabled)
        {
            offset -= ((speed * factor) * movementFactor) * Time.deltaTime;
            beltMaterial.SetTextureOffset("_MainTex", new Vector2(0, offset));

            if(increaseFactor > 0)
            {
                movementFactor = movementFactor + increaseFactor <= 1 ? movementFactor + increaseFactor * Time.deltaTime : 1;
            }
            else
            {
                movementFactor = 1;
            }
        }
        else
        {
            //movementFactor = Mathf.LerpUnclamped(movementFactor, 0, 0.7f * Time.deltaTime);

            movementFactor = movementFactor - decreaseFactor >= 0 ? movementFactor - decreaseFactor * Time.deltaTime : 0;

            if (movementFactor > 0)
            {
                offset -= ((speed * factor) * movementFactor) * Time.deltaTime;
                beltMaterial.SetTextureOffset("_MainTex", new Vector2(0, offset));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasEnergy && isEnabled || movementFactor > 0)
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, endPoint.position, speed * movementFactor * Time.deltaTime);
        }
    }

    public void SetEnergyState(string state)
    {
        if(state == "Energized")
        {
            if(!hasEnergy)
            {
                TurnOn();
            }

            hasEnergy = true;
        }
        else if (state == "NotEnergized")
        {
            if (!hasEnergy)
            {
                TurnOff();
            }

            hasEnergy = false;
        }
    }

    public void Interact()
    {
        if(!isEnabled)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOn()
    {
        isEnabled = true;

        if(hasEnergy)
        {
            mainSource.Play();

            secondarySource.PlayOneShot(startSound);
        }
    }

    public void TurnOff()
    {
        isEnabled = false;

        mainSource.Stop();

        secondarySource.PlayOneShot(stopSound);
    }
}
