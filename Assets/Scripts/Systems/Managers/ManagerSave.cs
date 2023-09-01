using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSave : MonoBehaviour
{
    public Transform _Player;
    [HideInInspector]
    public string _PlayerName = "Default Player";
    public bool debugActions = true;

    [Space(10)]
    [Header("Objects to save")]
    [Space(10)]
    public List<SaveableObject> dynamicObjectsToSave;
    public Transform dynamicObjectsParent;
    public List<SaveableObject> staticObjectsToSave;

    [Space(10)]
    [Header("UI Elements")]
    [Space(10)]
    public ManagerSavegamesUI savegamesUIManager;
    [Space(10)]
    public Transform holderLoadgameSaveSlots;
    public Transform holderSavegameSaveSlots;
    public GameObject savegameUIElement;
    public GameObject loadgameUIElement;

    public InputField newSavegameNameInputField;
    public Button NewSavegameConfirmButton;
    public Transform buttonCreateSave;

    private string currentPath;
    private string currentThumbnailPath;
    
    private string defaultSaveLoadPath;
    private IEnumerator screenshotThumbnailCoroutine;

    public UserData myData;
    private string _data;

    private Vector3 VPosition;
    private Quaternion VRotation;

    public string[] pathSplit;

    #region Singleton

    private static ManagerSave instance;

    public static ManagerSave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ManagerSave>();
            }

            return instance;
        }
    }

    #endregion

    private void Start()
    {
        string pathSaves = PersistentInformation.defaultSavegamePath;

        if (!Directory.Exists(pathSaves))
        {
            Directory.CreateDirectory(pathSaves);
        }

        defaultSaveLoadPath = pathSaves + "//SaveData_0.xml";
        currentThumbnailPath = pathSaves + "//SaveData_0thumbnail.png";
        myData = new UserData();
        string targetSaveLoad = PersistentInformation.targetSaveLoad;
        string targetSaveThumbnail = PersistentInformation.targetSaveThumbnail;
        currentPath = !string.IsNullOrEmpty(targetSaveLoad) && !string.IsNullOrWhiteSpace(targetSaveLoad) ? targetSaveLoad : defaultSaveLoadPath;
        currentThumbnailPath = !string.IsNullOrEmpty(targetSaveThumbnail) && !string.IsNullOrWhiteSpace(targetSaveThumbnail) ? targetSaveThumbnail : pathSaves + "//SaveData_0thumbnail.png";
        if (!PersistentInformation.isNewGame)
        {
            StartCoroutine(PreventDupeAndLoad());
        }

        ReloadSavegames();
    }

    private void OnDrawGizmos()
    {
        
    }

    private IEnumerator PreventDupeAndLoad()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LoadGame();
    }

    public void SetCurrentPathAndSave(string path)
    {
        currentPath = path;
        SaveGame();
    }

    public void CreatePathFromIndexAndSave(int saveIndex)
    {
        currentPath = PersistentInformation.defaultSavegamePath + "//SaveData_"+ saveIndex +".xml";
        currentThumbnailPath = PersistentInformation.defaultSavegamePath + "//SaveData_" + saveIndex + "thumbnail.png";
        SaveGame();
    }

    public void SetCurrentPathAndLoad(int loadIndex)
    {
        currentPath = PersistentInformation.defaultSavegamePath + "//SaveData_" + loadIndex + ".xml";
        currentThumbnailPath = PersistentInformation.defaultSavegamePath + "//SaveData_" + loadIndex + "thumbnail.png";
        LoadGame();
    }

    public void CreateNewSaveGame()
    {
        _PlayerName = newSavegameNameInputField.text;

        for (int indexLook = 0; indexLook < PersistentInformation.savegamesAmount - 1; indexLook++)
        {
            if (!new FileInfo(PersistentInformation.defaultSavegamePath + "//SaveData_" + indexLook + ".xml").Exists && !new FileInfo(PersistentInformation.defaultSavegamePath + "//SaveData_" + indexLook + "thumbnail.png").Exists)
            {
                currentPath = PersistentInformation.defaultSavegamePath + "//SaveData_" + indexLook + ".xml";
                currentThumbnailPath = PersistentInformation.defaultSavegamePath + "//SaveData_" + indexLook + "thumbnail.png";
                SaveGame();
                break;
            }
        }
    }

    public void UpdateNewSavegameConfirmButtonState()
    {
        if (!string.IsNullOrEmpty(newSavegameNameInputField.text) && !string.IsNullOrWhiteSpace(newSavegameNameInputField.text))
        {
            NewSavegameConfirmButton.gameObject.SetActive(true);
        }
        else
        {
            NewSavegameConfirmButton.gameObject.SetActive(false);
        }
    }

    public void EndEditNewSavegameName()
    {
        if (!string.IsNullOrEmpty(newSavegameNameInputField.text) && !string.IsNullOrWhiteSpace(newSavegameNameInputField.text))
        {
            CreateNewSaveGame();
            newSavegameNameInputField.text = string.Empty;
            ManagerGameplay.instance.CloseSaveGameNameScreen();
        }
    }

    [ContextMenu("Load")]
    public void LoadGame()
    {
        LoadXML();

        if (!string.IsNullOrEmpty(_data) && !string.IsNullOrWhiteSpace(_data))
        {
            foreach (SaveableObject objectToSave in dynamicObjectsToSave)
            {
                Destroy(objectToSave.gameObject);
            }

            dynamicObjectsToSave.Clear();

            myData = (UserData)DeserializeObject(_data);
            _PlayerName = myData.user.playerName;
            VPosition = myData.user.playerPosition;
            _Player.GetComponent<CharacterController>().enabled = false;
            _Player.transform.position = VPosition;
            VRotation = myData.user.playerRotation;
            _Player.transform.rotation = VRotation;
            _Player.GetComponent<CharacterController>().enabled = true;
            string[] allObjectsDatas = myData.allObjectsData.Split('%');
            myData.allObjectsData = myData.allObjectsData.Trim('%');
            string[] dynamicObjectsDataSaved = allObjectsDatas[0].Split('\n');
            string[] staticObjectsDataSaved = allObjectsDatas[1].Split('\n');
            foreach (string singleDataSaved in dynamicObjectsDataSaved)
            {
                if(!string.IsNullOrEmpty(singleDataSaved))
                {
                    string[] loadedDataInfo = singleDataSaved.Split('|');
                    GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/" + loadedDataInfo[0]), dynamicObjectsParent);
                    temp.GetComponent<SaveableObject>().SetCurrentData(singleDataSaved);
                    temp.GetComponent<SaveableObject>().LoadFromCurrentData();

                    if (debugActions)
                    {
                        Debug.Log("<color=green>Loaded</color> <color=red>" + loadedDataInfo[0] + "</color><color=green> sucessfuly!</color>");
                    }
                }
            }

            for(int i = 0; i < staticObjectsDataSaved.Length; i++)
            {
                string data = staticObjectsDataSaved[i];
                if (!string.IsNullOrEmpty(data))
                {
                    staticObjectsToSave[i].SetCurrentData(data);
                    staticObjectsToSave[i].LoadFromCurrentData();

                    if(debugActions)
                    {
                        Debug.Log("<color=green>Loaded</color> <color=black>" + staticObjectsToSave[i].name + "</color><color=green> sucessfuly!</color>");
                    }
                }
            }

            if (debugActions)
            {
                Debug.Log("<color=green>Load sucessfull : </color><color=blue>" + myData.user.playerName + "</color>");
            }
        }
    }

    [ContextMenu("Save")]
    public void SaveGame()
    {
        myData = new UserData();
        myData.user.playerPosition = _Player.transform.position;
        myData.user.playerRotation = _Player.transform.rotation;
        myData.user.playerName = _PlayerName;

        foreach(SaveableObject objectToSave in dynamicObjectsToSave)
        {
            objectToSave.UpdateDataToSaveToCurrentData();
            myData.allObjectsData += objectToSave.GetCurrentData + "\n";

            if (debugActions)
            {
                Debug.Log("<color=green>Saved</color> <color=red>" + objectToSave.transform.name + "</color><color=green> sucessfuly!</color>");
            }
        }

        myData.allObjectsData += "%";

        foreach (SaveableObject objectToSave in staticObjectsToSave)
        {
            objectToSave.UpdateDataToSaveToCurrentData();
            myData.allObjectsData += objectToSave.GetCurrentData + "\n";

            if (debugActions)
            {
                Debug.Log("<color=green>Saved</color> <color=red>" + objectToSave.transform.name + "</color><color=green> sucessfuly!</color>");
            } 
        }

        myData.allObjectsData += "%" + DateTime.Now.ToString();

        _data = SerializeObject(myData);
        CreateXML();

        if(debugActions)
        {
            Debug.Log("<color=green>Saved sucessfull : </color><color=blue>" + myData.user.playerName + "</color>");
        }

        screenshotThumbnailCoroutine = ScreenshotNoUI();
        StopCoroutine(screenshotThumbnailCoroutine);
        StartCoroutine(screenshotThumbnailCoroutine);
    }

    public void SaveCurrent()
    {
        FileInfo infoSave = new FileInfo(currentPath);
        FileInfo infoSaveThumb = new FileInfo(currentThumbnailPath);

        if(infoSave.Exists)
        {
            infoSave.Delete();
        }

        if (infoSaveThumb.Exists)
        {
            infoSaveThumb.Delete();
        }

        SaveGame();

        StartCoroutine(WaitForSaveEnd());
    }

    private IEnumerator WaitForSaveEnd()
    {
        while (new FileInfo(currentPath).Exists == false)
        {
            yield return null;
        }

        while (new FileInfo(currentThumbnailPath).Exists == false)
        {
            yield return null;
        }

        ManagerGameplay.instance.MainMenu();
    }

    public void ReloadSavegames()
    {
        buttonCreateSave.transform.SetParent(null);

        if (holderSavegameSaveSlots.childCount > 0)
        {
            for (int i = 0; i < holderSavegameSaveSlots.childCount; i++)
            {
                Destroy(holderSavegameSaveSlots.GetChild(i).gameObject);
                Destroy(holderLoadgameSaveSlots.GetChild(i).gameObject);
            }
        }

        for (int indexLook = 0; indexLook < (PersistentInformation.savegamesAmount - 1); indexLook++)
        {
            if (new FileInfo(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + ".xml").Exists && new FileInfo(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + "thumbnail.png").Exists)
            {
                UIElementSavegame newSavegameUIElement1 = Instantiate(loadgameUIElement, holderLoadgameSaveSlots).GetComponent<UIElementSavegame>();
                UIElementSavegame newSavegameUIElement2 = Instantiate(savegameUIElement, holderSavegameSaveSlots).GetComponent<UIElementSavegame>();

                newSavegameUIElement1.manager = savegamesUIManager;
                newSavegameUIElement2.manager = savegamesUIManager;

                newSavegameUIElement1.myIndex = indexLook;
                newSavegameUIElement2.myIndex = indexLook;

                newSavegameUIElement1.SetInfo(new SaveGame(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + ".xml", PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + "thumbnail.png"));
                savegamesUIManager.loadgameSaveSlots.Add(newSavegameUIElement1);
                newSavegameUIElement2.SetInfo(new SaveGame(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + ".xml", PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + "thumbnail.png"));
                savegamesUIManager.savegameSaveSlots.Add(newSavegameUIElement2);                
            }
        }

        buttonCreateSave.transform.SetParent(holderSavegameSaveSlots);

        buttonCreateSave.transform.SetAsLastSibling();

        savegamesUIManager.UpdateLoadGameButtonState();
    }

    private IEnumerator ScreenshotNoUI()
    {
        ManagerGameplay.instance.SavegameWindowObject.GetComponent<CanvasGroup>().alpha = 0;

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot(currentThumbnailPath, 1);

        ManagerGameplay.instance.SavegameWindowObject.GetComponent<CanvasGroup>().alpha = 1;

        while (new FileInfo(currentThumbnailPath).Exists == false)
        {
            yield return null;
        }

        while (new FileInfo(currentPath).Exists == false)
        {
            yield return null;
        }

        ReloadSavegames();
    }

    public string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return constructedString;
    }

    public byte[] StringToUTF8ByteArray(string characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(characters);
        return byteArray;
    }

    private string SerializeObject( object pObject)
    {
        string xmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return xmlizedString;
    }

    private object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }

    private void CreateXML()
    {
        currentPath = !string.IsNullOrEmpty(currentPath) && !string.IsNullOrWhiteSpace(currentPath) ? currentPath : defaultSaveLoadPath;

        if (!string.IsNullOrEmpty(currentPath) && !string.IsNullOrWhiteSpace(currentPath))
        {
            StreamWriter writer;
            FileInfo t = new FileInfo(currentPath);
            if (!t.Exists)
            {
                writer = t.CreateText();

                if (debugActions)
                {
                    Debug.Log("<color=green>Created xml file sucessfull at path : </color><color=blue>" + currentPath + "</color>");
                }
            }
            else
            {
                t.Delete();
                writer = t.CreateText();

                if (debugActions)
                {
                    Debug.Log("<color=red>Replaced </color><color=green>xml file sucessfull at path : </color><color=blue>" + currentPath + "</color>");
                }
            }

            writer.Write(_data);
            writer.Close();
        }
        else
        {
            Debug.LogError("The path to save the game is not valid, current path:" + currentPath);
        }
    }

    private void LoadXML()
    {
        currentPath = File.Exists(currentPath) ? currentPath : File.Exists(defaultSaveLoadPath) ? defaultSaveLoadPath : string.Empty;

        if (!string.IsNullOrEmpty(currentPath) && !string.IsNullOrWhiteSpace(currentPath))
        {
            StreamReader r = File.OpenText(currentPath);
            string _info = r.ReadToEnd();
            r.Close();
            _data = _info;

            if (debugActions)
            {
                Debug.Log("<color=blue>Loaded</color><color=green> xml file sucessfull at path : </color><color=blue>" + currentPath + "</color>");
            }
        }
        else
        {
            Debug.LogError("The path to load the game is not valid, current path:" + currentPath);
        }
    }

    public bool HasSaveGame
    {
        get
        {
            return new FileInfo(currentPath).Exists;
        }
    }

    [ContextMenu("Get all static saveable objects")]
    private void GetStaticSaveables()
    {
        if(staticObjectsToSave !=null)
        {
            staticObjectsToSave.Clear();
        }
        else
        {
            staticObjectsToSave = new List<SaveableObject>();
        }

        SaveableObject[] saveables = FindObjectsOfType(typeof(SaveableObject)) as SaveableObject[];

        for (int i = 0; i < saveables.Length; i++)
        {
            if(!saveables[i].isDynamic)
            {
                staticObjectsToSave.Add(saveables[i]);
            }
        }
    }
}