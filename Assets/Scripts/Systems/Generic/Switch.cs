using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : SaveableObject
{
    public Animator anim;
    public bool hasEnergy;
    [Space(10)]
    [Header("Signals")]
    
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueLeverUp;
    public string ParameterValueLeverDown;

    [Space(10)]
    [Header("Sounds")]
    public AudioSource mainSource;
    public AudioClip leverUpSound;
    public AudioClip leverDownSound;
    public AudioClip onEnergyOnSound;
    public AudioClip onEnergyOffSound;
    [Space(10)]
    [Header("Misc")]
    public bool useIndicatorOutput = true;
    public bool useIndicatorInput = true;
    public Material onMatOn;
    public Material onMatOff;
    public Material offMatOn;
    public Material offMatOff;
    public Renderer indicatorOutputOn;
    public Renderer indicatorOutputOff;
    public Renderer indicatorInputEnergy;

    private bool isPulled
    {
        get
        {
            return anim.GetBool("On");
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    private void Awake()
    {
        if(useIndicatorInput && indicatorInputEnergy && onMatOn && offMatOff)
        {
            indicatorInputEnergy.material = hasEnergy ? onMatOn : offMatOff;
        }

        if (indicatorOutputOn && indicatorOutputOff)
        {
            indicatorOutputOn.gameObject.SetActive(useIndicatorOutput);
            indicatorOutputOff.gameObject.SetActive(useIndicatorOutput);

            if (useIndicatorOutput && onMatOn && onMatOff && offMatOn && offMatOff)
            {
                indicatorOutputOn.material = isPulled ? onMatOn : onMatOff;
                indicatorOutputOff.material = isPulled ? offMatOn : offMatOff;
            }
            else if (useIndicatorOutput && onMatOn && onMatOff)
            {
                indicatorOutputOn.material = isPulled ? onMatOn : onMatOff;
            }
        }
    }

    public void Pull()
    {
        if (anim.GetBool("On"))
        {
            if (hasEnergy)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueLeverUp);
            }

            anim.SetBool("On", false);

            if(leverUpSound != null && mainSource != null)
            {
                mainSource.PlayOneShot(leverUpSound);
            }
        }
        else
        {
            if(hasEnergy)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueLeverDown);
            }

            anim.SetBool("On", true);

            if(leverDownSound !=null && mainSource != null)
            {
                mainSource.PlayOneShot(leverDownSound);
            }
        }

        if (useIndicatorOutput && onMatOn && onMatOff && offMatOn && offMatOff)
        {
            indicatorOutputOn.material = isPulled && hasEnergy ? onMatOn : onMatOff;
            indicatorOutputOff.material = !isPulled && hasEnergy ? offMatOn : offMatOff;
        }
        else if (useIndicatorOutput && onMatOn && onMatOff)
        {
            indicatorOutputOn.material = isPulled && hasEnergy ? onMatOn : onMatOff;
        }
    }

    public void SetEnergyState(string state)
    {
        if(state == "Energized")
        {
            if(!hasEnergy)
            {
                hasEnergy = true;

                if (onEnergyOnSound != null && mainSource != null)
                {
                    mainSource.PlayOneShot(onEnergyOnSound);
                }

                if (anim.GetBool("On"))
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueLeverDown);
                }
                else
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueLeverUp);
                }
            }
        }
        else if(state == "NotEnergized")
        {
            if(hasEnergy)
            {
                hasEnergy = false;

                if (onEnergyOffSound != null && mainSource != null)
                {
                    mainSource.PlayOneShot(onEnergyOffSound);
                }

                Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueLeverUp);
            }
        }

        if (useIndicatorOutput && onMatOn && onMatOff && offMatOn && offMatOff)
        {
            indicatorOutputOn.material = isPulled && hasEnergy ? onMatOn : onMatOff;
            indicatorOutputOff.material = !isPulled && hasEnergy ? offMatOn : offMatOff;
        }
        else if (useIndicatorOutput && onMatOn && onMatOff)
        {
            indicatorOutputOn.material = isPulled && hasEnergy ? onMatOn : onMatOff;
        }

        if (useIndicatorInput && indicatorInputEnergy && onMatOn && offMatOff)
        {
            indicatorInputEnergy.material = hasEnergy ? onMatOn : offMatOff;
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

        hasEnergy = bool.Parse(loadedData[1]);

        bool pulled = bool.Parse(loadedData[0]);

        anim.SetBool("On" ,pulled);

        if (useIndicatorInput && indicatorInputEnergy && onMatOn && offMatOff)
        {
            indicatorInputEnergy.material = hasEnergy ? onMatOn : offMatOff;
        }

        if (indicatorOutputOn && indicatorOutputOff)
        {
            indicatorOutputOn.gameObject.SetActive(useIndicatorOutput);
            indicatorOutputOff.gameObject.SetActive(useIndicatorOutput);

            if (useIndicatorOutput && onMatOn && onMatOff && offMatOn && offMatOff)
            {
                indicatorOutputOn.material = isPulled ? onMatOn : onMatOff;
                indicatorOutputOff.material = isPulled ? offMatOn : offMatOff;
            }
            else if (useIndicatorOutput && onMatOn && onMatOff)
            {
                indicatorOutputOn.material = isPulled ? onMatOn : onMatOff;
            }
        }
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = anim.GetBool("On").ToString() + "|" + hasEnergy.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
