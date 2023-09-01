using UnityEngine;
using System.Collections;

public class SecurityCamera : MonoBehaviour
{

    string stateAnimationName = "Security_Camera_Idle_Loop";
    public float startDelay = 1;

    private void Start()
    {
        if (startDelay > 0)
        {
            StartCoroutine(StartDelay());
        }
        else
        {
            GetComponent<Animator>().CrossFade(stateAnimationName, 1);
        }
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);

        GetComponent<Animator>().CrossFade(stateAnimationName, 1);
    }

    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

}
