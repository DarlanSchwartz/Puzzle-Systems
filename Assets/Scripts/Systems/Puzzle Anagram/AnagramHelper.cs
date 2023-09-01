using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnagramHelper : MonoBehaviour
{
    public List<Transform> Letters;
    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVXYWZ";
    public Anagram father;

    [ContextMenu("Set")]
    public void Help()
    {
        for (int i = 0; i < alphabet.Length; i++)
        {
            Letters[i].name = alphabet[i].ToString();
        }

        foreach (Transform letter in Letters)
        {
            letter.GetChild(0).GetComponent<Text>().text = letter.name;
        }
    }
}
