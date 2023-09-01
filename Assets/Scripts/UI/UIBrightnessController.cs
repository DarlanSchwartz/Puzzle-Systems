using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UIBrightnessController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Image referenceImage;
    public Slider sliderInput;
    public Text outputText;
    public float currentBrightnessValue;
    public byte percentage;
    public void UpdateBrightness()
    {
        currentBrightnessValue =  3.3f * (sliderInput.value - 0.3f);
        postProcessVolume.profile.GetSetting<ColorGrading>().gamma.value.Set(currentBrightnessValue, currentBrightnessValue, currentBrightnessValue, currentBrightnessValue);
        percentage = ((byte)(sliderInput.value * 255));
        referenceImage.color = new Color32(percentage, percentage, percentage,255);
        outputText.text = ((int)(sliderInput.value * 200)).ToString() + "%";
    }
}
