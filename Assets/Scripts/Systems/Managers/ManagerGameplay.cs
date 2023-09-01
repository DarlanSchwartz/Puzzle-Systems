using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ManagerGameplay : MonoBehaviour
{
    public bool isOpen;
    public FirstPersonController controller;
    [Space(2, order = 0)]
    [Header("Menu and Windows", order = 1)]
    [Space(10 ,order = 2)]
    public Transform menuObject;
    public Transform AreYouSureObject;
    public Transform LoadGameMenuObject;
    public Transform SavegameWindowObject;
    public Transform SavegameNameObject;

    #region Singleton
    private static ManagerGameplay _instance;

    public static ManagerGameplay instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ManagerGameplay>();
            }

            return _instance;
        }
    }
    #endregion

    public void Save()
    {
        ManagerSave.Instance.SaveGame();
        Close();
    }

    public void Load()
    {
        ManagerSave.Instance.LoadGame();
        Close();
    }

    public void Open()
    {
        menuObject.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        controller.enabled = false;
        Time.timeScale = 0;
        isOpen = true;
    }

    public void Close()
    {
        menuObject.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller.enabled = true;
        Time.timeScale = 1;
        isOpen = false;
    }

    public void Interact()
    {
        if(isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void MainMenu()
    {
        Close();
        PersistentInformation.targetSceneIndex = 0;
        SceneManager.LoadScene(1);
    }

    public void OpenLoadGameScreen()
    {
        menuObject.gameObject.SetActive(false);
        LoadGameMenuObject.gameObject.SetActive(true);
        ManagerSave.Instance.ReloadSavegames();
        ManagerSound.ClickSound();
    }

    public void CloseLoadGameScreen()
    {
        LoadGameMenuObject.gameObject.SetActive(false);
        menuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void OpenSaveGameScreen()
    {
        menuObject.gameObject.SetActive(false);
        SavegameWindowObject.gameObject.SetActive(true);
        ManagerSave.Instance.ReloadSavegames();
        ManagerSound.ClickSound();
    }

    public void SaveAndExitMainMenu()
    {
        menuObject.gameObject.SetActive(false);
        ManagerSave.Instance.SaveCurrent();
        ManagerSound.ClickSound();
    }

    public void CloseSaveGameScreen()
    {
        SavegameWindowObject.gameObject.SetActive(false);
        menuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void OpenSaveGameNameScreen()
    {
        SavegameWindowObject.gameObject.SetActive(false);
        SavegameNameObject.gameObject.SetActive(true);
        ManagerSound.ClickSound();
    }

    public void CloseSaveGameNameScreen()
    {
        SavegameNameObject.gameObject.SetActive(false);
        SavegameWindowObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TryQuit()
    {
        AreYouSureObject.gameObject.SetActive(true);
        menuObject.gameObject.SetActive(false);
        ManagerSound.ClickSound();
    }

    public void CancelQuit()
    {
        AreYouSureObject.gameObject.SetActive(false);
        menuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void DeleteSaveGame(UIElementSavegame slot)
    {
        if(slot.referenceSaveGame !=null)
        {
            File.Delete(slot.referenceSaveGame.savePath);
        }
    }

    public void CreateNewSavegame()
    {
        OpenSaveGameNameScreen();
    }
}
