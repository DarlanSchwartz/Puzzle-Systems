using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerLoadingScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public Text loadingCompleteText;
    public Text loadingText;
    public UIAnimationLoadingIcon loadingIcon;

    IEnumerator LoadSceneAsync()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(PersistentInformation.targetSceneIndex);

        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if(PersistentInformation.targetSceneIndex == 2)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    loadingCompleteText.enabled = true;

                    if(!loadingCompleteText.gameObject.GetComponent<Animation>().isPlaying)
                    {
                        loadingCompleteText.gameObject.GetComponent<Animation>().Play();
                    }

                    if(loadingText && loadingIcon)
                    {
                        Destroy(loadingText.gameObject);
                        Destroy(loadingIcon.gameObject);
                    }

                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                }
            }
            else
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
