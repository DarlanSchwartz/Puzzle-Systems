using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ManagerSound : MonoBehaviour
{
    public AudioSource UISource;
    public AudioSource musicSource;
    public Slider musicSlider;
    public Slider ambienceSlider;
    public AudioMixer mixerMusic;
    public AudioMixer mixerAmbience;
    public AudioClip clickSound;
    public AudioClip clickSound2;

    private static AudioClip staticClickSound;
    private static AudioClip staticClickSound2;
    private static AudioSource staticUISource;

    void Start()
    {
        staticClickSound = clickSound;
        staticClickSound2 = clickSound2;
        staticUISource = UISource;
        
        if(ambienceSlider && musicSlider)
        {
            mixerMusic.SetFloat("MasterVolume", musicSlider.value - 80);
            mixerAmbience.SetFloat("MasterVolume", ambienceSlider.value - 80);
        }
    }

    public static void ClickSound()
    {
        staticUISource.PlayOneShot(staticClickSound);
    }

    public static void ClickSound2()
    {
        staticUISource.PlayOneShot(staticClickSound2);
    }

    public void UpdateMusicVolume()
    {
        mixerMusic.SetFloat("MasterVolume", musicSlider.value -80);
    }

    public void UpdateAmbienceVolume()
    {
        mixerAmbience.SetFloat("MasterVolume", ambienceSlider.value - 80);
    }
}
