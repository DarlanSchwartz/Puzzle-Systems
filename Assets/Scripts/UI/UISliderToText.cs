using UnityEngine;
using UnityEngine.UI;

public class UISliderToText : MonoBehaviour
{
    public Slider inputSlider;
    public Text outputText;
    public string additionals;

    public void UpdateValues()
    {
        outputText.text = inputSlider.value.ToString() + additionals;
    }
}
