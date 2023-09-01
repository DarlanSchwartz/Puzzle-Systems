using UnityEngine;

public class Padlock : SaveableObject
{
    public bool saveRigidbodyInfo;
    [Space(10)]
    [Header("States")]
    public bool locked = true;
    public string currentCode;
    public string correctCode;
    [Space(10)]
    [Header("References")]
    public Animator anim;
    public PadlockCircle first;
    public PadlockCircle second;
    public PadlockCircle third;
    [Space(10)]
    [Header("Songs")]
    public AudioSource mainSource;
    public AudioClip unlockSound;
    public AudioClip rotateSound;
    [Space(10)]
    [Header("Signals")]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnUnlock;

    private int unlockHash = Animator.StringToHash("Unlock");
    private int resetHash = Animator.StringToHash("Reset");

    public void UpdateCode()
    {
        currentCode = first.currentNumber.ToString() + second.currentNumber.ToString() + third.currentNumber.ToString();

        if(currentCode == correctCode)
        {
            Unlock();
        }
    }

    public void Unlock()
    {
       if(locked)
        {
            anim.SetTrigger(unlockHash);
            locked = false;

            if(mainSource && unlockSound)
            {
                mainSource.PlayOneShot(unlockSound);
            }

            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnUnlock);
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

        if (!isDynamic)
        {
            currentCode = loadedData[0];
            locked = bool.Parse(loadedData[1]);
        }
        else
        {
            base.LoadFromCurrentData();
            currentCode = loadedData[7];
            locked = bool.Parse(loadedData[8]);

            if(saveRigidbodyInfo)
            {
                Rigidbody rigid = GetComponent<Rigidbody>();
                rigid.useGravity = bool.Parse(loadedData[9]);
                rigid.isKinematic = bool.Parse(loadedData[10]);
            }
        }

        if (!locked)
        {
            anim.SetTrigger(resetHash);
            anim.SetTrigger(unlockHash);
        }

        first.currentNumber = currentCode[0];
        second.currentNumber = currentCode[1];
        third.currentNumber = currentCode[2];

        first.ResetToCurrent();
        second.ResetToCurrent();
        third.ResetToCurrent();
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        if(!isDynamic)
        {
            dataToSave = currentCode + "|" + locked.ToString();
        }
        else
        {
            base.UpdateDataToSaveToCurrentData();

            dataToSave = dataToSave + "|" + currentCode + "|" + locked.ToString();

            if (saveRigidbodyInfo)
            {
                Rigidbody rigid = GetComponent<Rigidbody>();

                dataToSave = dataToSave + "|" + rigid.useGravity.ToString() + "|" + rigid.isKinematic.ToString();
            }
        }
    }
}
