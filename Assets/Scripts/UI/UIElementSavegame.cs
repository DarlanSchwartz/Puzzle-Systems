using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class UIElementSavegame : MonoBehaviour
{
    public Image savegameThumbnail;
    public Text savegameDayTime;
    public Text savegameTitle;
    public SaveGame referenceSaveGame;
    public ManagerSavegamesUI manager;
    public int myIndex;
    private IEnumerator updateInfoCoroutine;

    private void Start()
    {
        updateInfoCoroutine = UpdateInfoCoroutine();
    }

    public void SetInfo(SaveGame saveGame)
    {
        savegameThumbnail.sprite = saveGame.thumbnail;
        savegameDayTime.text = saveGame.saveTime;
        savegameTitle.text = saveGame.saveName;
        referenceSaveGame = saveGame;
    }

    private IEnumerator UpdateInfoCoroutine()
    {
        while(!new FileInfo(referenceSaveGame.savePath).Exists)
        {
            yield return null;
        }

        while (!new FileInfo(referenceSaveGame.thumbnailSavePath).Exists)
        {
            yield return null;
        }

        referenceSaveGame = new SaveGame(referenceSaveGame.savePath, referenceSaveGame.thumbnailSavePath);
        
        SetInfo(referenceSaveGame);
    }

    public void Delete(bool inGameplay = true)
    {
        File.Delete(referenceSaveGame.thumbnailSavePath);
        File.Delete(referenceSaveGame.savePath);

        ManagerSound.ClickSound2();
        Destroy(gameObject);

        if (inGameplay)
        {
            manager.saveManager.ReloadSavegames();
        }
        else
        {
            ManagerMenu.Instance.ReloadSaves();
        }
    }

    public void Save()
    {
        File.Delete(referenceSaveGame.thumbnailSavePath);
        File.Delete(referenceSaveGame.savePath);
        ManagerSave.Instance.CreatePathFromIndexAndSave(myIndex);
        ManagerSound.ClickSound();
    }

    public void Load()
    {
        PersistentInformation.targetSaveLoad = PersistentInformation.defaultSavegamePath + "//SaveData_" + myIndex + ".xml";
        PersistentInformation.targetSaveThumbnail = PersistentInformation.defaultSavegamePath + "//SaveData_" + myIndex + "thumbnail.png";
        PersistentInformation.isNewGame = false;
        PersistentInformation.targetSceneIndex = 2;
        SceneManager.LoadScene(1);
        //ManagerSave.Instance.SetCurrentPathAndLoad(myIndex);
    }

    public void LoadOnMenu()
    {
        ManagerMenu.Instance.LoadGame(referenceSaveGame.savePath);
    }
}
