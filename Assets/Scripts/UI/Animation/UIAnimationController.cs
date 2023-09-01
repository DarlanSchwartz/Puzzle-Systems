using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public Animation animationTarget;
    public string[] animations;
    public int currentIndex;
    
    public void SetCurrentAndPlay(int index)
    {
        if(index == currentIndex)
        {
            return;
        }

        currentIndex = index;

        animationTarget.CrossFade(animations[currentIndex]);
    }
}
