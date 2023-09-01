using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Anagram : MonoBehaviour
{
    public string currentWord = string.Empty;
    public string correctWord = string.Empty;
    public List<AnagramObject> anagramObjects;
    public List<Button> buttonLetters;

    public bool selecting = false;
    public char selectedLetter = ' ';

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartSelection(Transform from)
    {
        if(!selecting)
        {
            foreach (AnagramObject anagramObject in anagramObjects)
            {
                if (anagramObject.currentLetter.ToString() == from.GetChild(0).GetComponent<Text>().text)
                {
                    anagramObject.SetLetter(' ');
                }
            }

            selectedLetter = char.Parse(from.GetChild(0).GetComponent<Text>().text);

            foreach(Button buttonLetter in buttonLetters)
            {
                buttonLetter.interactable = false;
            }

            selecting = true;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1) && selecting)
        {
            EndSelection();
        }
    }

    public string Read
    {
        get
        {
            string readedString = string.Empty;

            foreach (AnagramObject anagramObject in anagramObjects)
            {
                readedString += anagramObject.currentLetter;
            }

            return readedString;
        }
    }

    public void Substitute(char selected, char to)
    {
        foreach (AnagramObject anagramObject in anagramObjects)
        {
            if(anagramObject.myLetter == selected)
            {
                anagramObject.SetLetter(to);
                anagramObject.Unlit();
            }
        }

        EndSelection();
    }

    public void Selected(string letter)
    {
        if(selecting)
        {
            Substitute(char.Parse(letter), selectedLetter);
        }
    }

    public void EndSelection()
    {
        if(selecting)
        {
            selectedLetter = ' ';

            selecting = false;

            foreach (Button buttonLetter in buttonLetters)
            {
                buttonLetter.interactable = true;
            }
        }

        currentWord = Read;

        if(currentWord == correctWord)
        {
            CompletedAnagram();
        }
    }

    private void CompletedAnagram()
    {
        throw new NotImplementedException();
    }

    public void Lit(string current)
    {
        if(selecting)
        {
            foreach (AnagramObject anagramObject in anagramObjects)
            {
                if (anagramObject.myLetter.ToString() == current)
                {
                    anagramObject.Lit();
                }
                else
                {
                    anagramObject.Unlit();
                }
            }
        }
    }
}
