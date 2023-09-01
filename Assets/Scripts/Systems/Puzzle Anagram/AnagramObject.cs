using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnagramObject : MonoBehaviour
{
    public char currentLetter;
    public char myLetter = 'a';
    public Image litObject;
    public Text output;
    public Image simbol;
    public Color transparent;

    public void Lit()
    {
        litObject.enabled = true;
    }

    public void Unlit()
    {
        litObject.enabled = false;
    }

    public void SetLetter(char letter)
    {
        output.text = letter.ToString();
        currentLetter = letter;

        if(!char.IsWhiteSpace(letter))
        {
            simbol.color = transparent;
        }
        else
        {
            simbol.color = Color.white;
        }
    }
}
