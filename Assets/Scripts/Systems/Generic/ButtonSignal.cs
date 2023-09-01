using UnityEngine;
public class ButtonSignal : MonoBehaviour
{
    [Space(10)]
    [Header("Signals")]
    public Transform receiver;
    public string methodName;
    public MessageType messageType = MessageType.VoidRun;
    public string parameterValue;
    [Space(10)]
    [Header("Sounds")]
    public AudioClip pressSound;
    public AudioSource mainSource;

    private void OnDrawGizmos()
    {
        
    }

    public void Press()
    {
        if(mainSource !=null && pressSound !=null)
        {
            mainSource.PlayOneShot(pressSound);
        }

        Messager.RunVoid(receiver, methodName, messageType.ToString(), parameterValue);
    }
}