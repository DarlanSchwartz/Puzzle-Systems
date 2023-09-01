using System.Collections.Generic;
using UnityEngine;

public class SliderPuzzle : SaveableObject
{
    public List<SliderPuzzlePiece> pieces;
    public bool isSolved = false;
    public bool canMovePiecesAfterSolved = false;
    public float pieceMoveSpeed = 1f;
    [Space(10)]
    [Header("Signals")]

    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string ParameterValueOnComplete;

    [Space(10)]
    [Header("Songs")]
    [Space(10)]
    [SerializeField] private AudioSource mainSource;
    [SerializeField] private AudioClip completionSound;

    private void OnDrawGizmos()
    {
        
    }

    public void CheckCompletion()
    {
        foreach(SliderPuzzlePiece piece in pieces)
        {
            if(!piece.IsInPlace() && piece.gameObject.activeSelf)
            {
                return;
            }
        }

        if(!canMovePiecesAfterSolved)
        {
            foreach (SliderPuzzlePiece piece in pieces)
            {
                piece.canInteract = false;
            }
        }

        if(mainSource && completionSound)
        {
            mainSource.PlayOneShot(completionSound);
        }

        if(receiver)
        {
            Messager.RunVoid(receiver, methodName, messageType.ToString(), ParameterValueOnComplete);
        }
        else
        {
            Debug.Log("No receiver on Slider Puzzle" + name);

#if UNITY_EDITOR
            UnityEditor.Selection.activeTransform = transform; 
#endif
        }
    }

    [ContextMenu("Solve instantly.")]
    public void SolvePuzzleInstantly()
    {
        foreach (SliderPuzzlePiece piece in pieces)
        {
            piece.transform.localPosition = piece.correctPlace;
        }

        CheckCompletion();
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        isSolved = bool.Parse(dataToSave);
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = isSolved.ToString();
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
