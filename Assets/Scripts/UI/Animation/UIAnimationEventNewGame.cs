using UnityEngine;

public class UIAnimationEventNewGame : MonoBehaviour
{
    public ManagerMenu manager;

    public void NewGameAnimationEndEvent()
    {
        manager.StartNewGame();
    }
}
