using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectOrder : SaveableObject
{
    [Header("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", order = -1)]
    [Header("States and conditions", order = 0)]
    [Space(10, order = 1)]
    [Tooltip("The current inputed code on the puzzle.")]
    public string currentCode = "";
    [Tooltip("Is the puzzle complete?")]
    public bool done = false;
    [Tooltip("The correct code to complete the puzzle")]
    public string correctCode = "";
    [Tooltip("Should be checked if the puzzle is completed when the current code changes?")]
    public bool tryOnAdd = false;
    [Tooltip("Should the buttons be reset when all the buttons are pressed?")]
    public bool autoReset;
    [Tooltip("All the buttons that sends messages to this puzzle.")]
    public List<ButtonSignalAnimated> buttonsControllers;
    [Header("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", order = 2)]
    [Header("On Complete",order =3)]
    [Space(10, order = 4)]
    [Tooltip("Should the buttons be reset when the puzzle gets completed?")]
    public bool resetButtons;
    [Tooltip("Should the buttons be disabled when the puzzle gets completed?")]
    public bool disableButtons;
    [Header("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", order = 5)]
    [Header("Signals", order = 6)]
    [Space(10, order = 7)]
    [Tooltip("The transform that will receive the messages from the events of this button.")]
    public Transform receiver;
    [Tooltip("The Method that will be called on the receiver when the puzzled gets completed.")]
    public string methodName;
    [Tooltip("The type of the parameter that will be sent as a message on the receiver.\n Void run means that it will only be called the method without parameters.")]
    public MessageType messageType = MessageType.VoidRun;
    [Tooltip("The value of the parameter that will be sent to the receiver when the puzzled gets completed.")]
    public string parameterValueOnComplete;

    public void AddCode(string buttonCode)
    {
        if(!done)
        {
            currentCode += buttonCode;

            if (tryOnAdd)
            {
                TryCurrent();
            }
        }
    }

    public void TryCurrent()
    {
        if (currentCode == correctCode)
        {
            PuzzleDone();
            return;
        }

        if(autoReset && AllButtonsPressed)
        {
            Reset();
        }
    }

    public bool AllButtonsPressed
    {
        get
        {
            foreach (ButtonSignalAnimated button in buttonsControllers)
            {
                if (!button.pressed)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public void PuzzleDone()
    {
        if(!done)
        {
            done = true;

            Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValueOnComplete);

            if (resetButtons && disableButtons)
            {
                DisableAndResetButtons();
                return;
            }

            if (resetButtons)
            {
                ResetButtons();
            }

            if(disableButtons)
            {
                DisableButtons();
            }
        }
    }

    private void Reset()
    {
        ResetButtons();

        currentCode = string.Empty;
    }

    public void ResetButtons()
    {
        foreach(ButtonSignalAnimated button in buttonsControllers)
        {
            button.Reset(true);
        }
    }

    public void DisableButtons()
    {
        foreach (ButtonSignalAnimated button in buttonsControllers)
        {
            button.Enabled = false;
        }
    }

    public void DisableAndResetButtons()
    {
        foreach (ButtonSignalAnimated button in buttonsControllers)
        {
            button.Enabled = false;
            button.Reset(true);
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
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = currentCode.ToString() + "|"  + done.ToString();
    }
}
