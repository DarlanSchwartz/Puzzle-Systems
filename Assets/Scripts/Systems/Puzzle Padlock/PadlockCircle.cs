using UnityEngine;

public class PadlockCircle : MonoBehaviour
{
    [Space(10)]
    [Header("States")]
    [Range(0,2)]public int myIndex;
    public int currentNumber;
    public Padlock manager;
    public Animator anim;
    [Space(10)]
    [Header("Songs")]
    public AudioSource mainSource;

    private void OnMouseDown()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (!anim.IsInTransition(0) && manager.locked)
        {
            int target = currentNumber == 9 ? 0 : currentNumber + 1;

            currentNumber = target;

            int hash = Animator.StringToHash(target.ToString());

            anim.CrossFade(hash, 0.5f, 0);

            manager.UpdateCode();

            if (mainSource && manager.rotateSound)
            {
                mainSource.PlayOneShot(manager.rotateSound);
            }
        }
    }

    public void ResetToCurrent()
    {
        anim.CrossFade(Animator.StringToHash(currentNumber.ToString()), 0, 0);
    }
}
