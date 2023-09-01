using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSignalAnimated : SaveableObject
{
    [Header("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", order = -1)]
    [Header("States and conditions", order = 0)]
    [Space(10, order = 1)]
    [Tooltip("Can you interact with the button if it is pressed?")]
    public bool canPressIfPressed = false;
    [Tooltip("Can you press the button if an animation is playing?")]
    public bool canPressOnTransition = true;
    [Tooltip("Is the button pressed?.")]
    public bool pressed = false;
    [Tooltip("Should the button reset to its default state when it gets pressed?.\nNotice that this option will only sort effect if the option can press if pressed is turned on.")]
    public bool resetOnPressIfPressed = true;
    [Tooltip("The animator that will be used to trigger this button animations.")]
    public Animator anim;
    [Header("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", order = 2)]
    [Header("Signals", order = 3)]
    [Space(10, order = 4)]
    [Tooltip("Should the button send signals if it gets pressed when it is already pressed.")]
    public bool sendSignalIfPressed = false;
    [Tooltip("The delay that the message will take to be sent to the receiver.\nThis is set default to 0.25 because it its the default animation transition time.")]
    public float messageDelay = 0.25f;
    [Tooltip("The transform that will receive the messages from the events of this button.")]
    public Transform receiver;
    [Tooltip("The Method that will be called on the receiver.")]
    public string methodName;
    [Tooltip("The type of the parameter that will be sent as a message on the receiver.\n Void run means that it will only be called the method without parameters.")]
    public MessageType messageType = MessageType.VoidRun;
    [Tooltip("The value of the parameter that will be sent to the receiver when the button is on its default state and gets pressed.")]
    public string parameterValueOnPress;
    [Tooltip("The value of the parameter that will be sent to the receiver when the button is on its pressed state and gets pressed.")]
    public string parameterValueOnReset;
    [Header("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", order = 5)]
    [Header("Sounds", order = 6)]
    [Space(10, order = 7)]
    [Tooltip("The sound that will be played when the button is pressed when it isnt pressed.")]
    public AudioClip pressSound;
    [Tooltip("The sound that will be played when the button is pressed when it is pressed.")]
    public AudioClip resetSound;
    [Tooltip("The audiosource that will play this button sounds.")]
    public AudioSource mainSource;
    
    private int pressedHash = Animator.StringToHash("Pressed");
    private int resetHash = Animator.StringToHash("Reset");
    private Coroutine delayPush;

    private void OnDrawGizmos()
    {

    }

    private void OnMouseDown()
    {
        Press();
    }

    public void Press()
    {
        if(!Enabled || pressed && !canPressIfPressed || !canPressOnTransition && anim.IsInTransition(0))
        {
            return;
        }

        if (mainSource)
        {
            if(!pressed && pressSound)
            {
                mainSource.PlayOneShot(pressSound);
            }
            else if(pressed && resetSound)
            {
                mainSource.PlayOneShot(resetSound);
            }
        }
        
        if(pressed)
        {
            if (resetOnPressIfPressed && pressed)
            {
                anim.SetBool(pressedHash, false);
                pressed = false;

                if (sendSignalIfPressed)
                {
                    Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueOnReset);
                }               
            }
        }
        else
        {
            anim.SetBool(pressedHash, true);
            pressed = true;
            if(messageDelay == 0)
            {
                Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueOnPress);
            }
            else
            {
                if(delayPush == null)
                {
                    delayPush = StartCoroutine(DelayPush());
                }
            }
        }
    }

    private IEnumerator DelayPush()
    {
        yield return new WaitForSeconds(messageDelay);

        Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueOnPress);

        delayPush = null;
    }

    public void Reset(bool playSound)
    {
        anim.SetBool(pressedHash, false);
        pressed = false;

        if(playSound && mainSource && resetSound)
        {
            mainSource.PlayOneShot(resetSound);
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

        pressed = bool.Parse(loadedData[0]);
        Enabled = bool.Parse(loadedData[1]);

        anim.SetTrigger(resetHash);
        anim.SetBool(pressedHash, pressed);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = pressed.ToString() + "|" + Enabled.ToString();
    }

    public bool Enabled { get; set; } = true;
}
