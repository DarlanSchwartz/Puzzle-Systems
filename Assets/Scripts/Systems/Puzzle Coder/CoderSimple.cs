using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CoderSimple : SaveableObject
{
    [Space(10)]
    [Header("States and conditions")]
    public int code = 123;
    private string currentCode;
    private int codeLenght;
    public Text codeDisplay;
    public bool disableOnUnlock = true;
    private bool isEnabled = true;
    private bool unlocking = false;
    [Range(0, 60)]
    [Tooltip("This value controls how long it will take to send the signal after putting the correct code!")]
    public float unlockDelay;
    [Space(10)]
    [Header("Songs")]
    public AudioSource mainSource;
    public AudioClip CorrectClip;
    public AudioClip IncorrectClip;
    [Space(10)]
    [Header("Signals")]
    [Space(10)]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueCorrectCode;
    public string ParameterValueIncorrectCode;

    private void OnDrawGizmos()
    {
        
    }

    private void Awake()
    {
        currentCode = "";
    }

    public void AddNumber(int number)
    {
        if (!isEnabled)
            return;

        if (currentCode.Length < codeLenght)
        {
            currentCode += number;

            codeDisplay.text = currentCode;
        }
    }

    public void TryCurrent()
    {
        if (!isEnabled)
            return;

        int parsedCode = 0;

        if(int.TryParse(currentCode, out parsedCode))
        {
            TryCode(parsedCode);
        }
    }

    public void Cancel()
    {
        if (!isEnabled)
            return;

        currentCode = string.Empty;

        codeDisplay.text = string.Empty;
    }

    public void TryCode(int codeTry)
    {
        if(codeTry == code)
        {
            Unlock();
        }
        else
        {
            WrongCode();
        }
    }

    private void WrongCode()
    {
        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueIncorrectCode);
        currentCode = string.Empty;
        codeDisplay.text = string.Empty;
        
        if(mainSource !=null && IncorrectClip !=null)
        {
            mainSource.PlayOneShot(IncorrectClip);
        }
    }

    private void Unlock()
    {
        if(unlockDelay > 0)
        {
            if(!unlocking)
            {
                StartCoroutine(UnlockAfterSeconds(unlockDelay));
                return;
            }
            else
            {
                Debug.LogError("Tryed to unlock when unlocking!");
            }
        }
        else
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCorrectCode);

            if (mainSource && CorrectClip)
            {
                mainSource.PlayOneShot(CorrectClip);
            }

            if (disableOnUnlock)
            {
                DisableCoder();
            }
        }
    }

    IEnumerator UnlockAfterSeconds(float seconds)
    {
        unlocking = true;

        if (mainSource && CorrectClip)
        {
            mainSource.PlayOneShot(CorrectClip);
        }

        if (disableOnUnlock)
        {
            DisableCoder();
        }

        yield return new WaitForSeconds(seconds);

        Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueCorrectCode);

        unlocking = false;
    }

    public void EnableCoder()
    {
        isEnabled = true;
    }

    public void DisableCoder()
    {
        isEnabled = false;
    }

    public void Reset()
    {
        unlocking = false;
        currentCode = string.Empty;
        codeDisplay.text = string.Empty;
        isEnabled = true;
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        codeLenght = code.ToString().Length;
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        currentCode = loadedData[0];

        codeDisplay.text = currentCode;

        isEnabled = bool.Parse(loadedData[1]);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = currentCode + "|" + isEnabled.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
