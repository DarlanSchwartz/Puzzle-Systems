using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerMenu : MonoBehaviour
{
    public Transform AreYouSureObject;
    public Transform CreditsObject;
    public Transform OptionsObject;
    public Transform OptionsObjectAudio;
    public Transform OptionsObjectQuality;
    public Transform OptionsObjectBrightness;
    public Transform OptionsObjectDisplay;
    public Transform LoadGameMenuObject;
    public Transform MainMenuObject;
    

    public ConfigurationUIElements UISettings;
    public RectTransform SavegamesHeader;
    public GameObject loadgameUIElement;
    public Transform holderLoadgameSaveSlots;
    public ManagerSavegamesUI savegamesUIManager;
    public Animation startNewGameAnimation;
    public Animation loadingIdicator;
    public Text loadingText;
    public Text loadingCompleteText;
    public Image loadingIcon;
    public string[] configurationsInfo;

    #region Singleton

    private static ManagerMenu instance;

    public static ManagerMenu Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ManagerMenu>();
            }

            return instance;
        }
    }

    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Resolution[] resolutions = Screen.resolutions;

        string[] resolutionsConversion = new string[resolutions.Length];

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionsConversion[i] = resolutions[i].width + "x" + resolutions[i].height;
        }

        List<string> resolutionsConversionList = new List<string>();

        foreach (string value in resolutionsConversion)
        {
            if (!string.IsNullOrEmpty(value) && value != "x" && !resolutionsConversionList.Contains(value))
            {
                resolutionsConversionList.Add(value);
            }
        }

        string[] finalResolutions = resolutionsConversionList.ToArray();

        UISettings.resolutionsContainer.SetCurrentOptions(finalResolutions);

        LoadConfiguration();

        configurationsInfo.Initialize();

        PersistentInformation.targetSaveLoad = string.Empty;
        PersistentInformation.targetSaveThumbnail = string.Empty;
        PersistentInformation.isNewGame = false;
        PersistentInformation.targetSceneIndex = 0;
        PersistentInformation.defaultSavegamePath = Application.dataPath + "/Savegames";

        ReloadSaves();
    }

    public void ReloadSaves()
    {
        SavegamesHeader.SetParent(null);

        if (holderLoadgameSaveSlots.childCount > 0)
        {
            for (int i = 0; i < holderLoadgameSaveSlots.childCount; i++)
            {
                Destroy(holderLoadgameSaveSlots.GetChild(i).gameObject);
            }
        }

        for (int indexLook = 0; indexLook < (PersistentInformation.savegamesAmount - 1); indexLook++)
        {
            if (new FileInfo(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + ".xml").Exists && new FileInfo(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + "thumbnail.png").Exists)
            {
                UIElementSavegame newSavegameUIElement1 = Instantiate(loadgameUIElement, holderLoadgameSaveSlots).GetComponent<UIElementSavegame>();
                newSavegameUIElement1.manager = savegamesUIManager;
                newSavegameUIElement1.myIndex = indexLook;
                newSavegameUIElement1.SetInfo(new SaveGame(PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + ".xml", PersistentInformation.defaultSavegamePath + "\\SaveData_" + indexLook + "thumbnail.png"));
                savegamesUIManager.loadgameSaveSlots.Add(newSavegameUIElement1);
            }
        }

        SavegamesHeader.SetParent(holderLoadgameSaveSlots);
        SavegamesHeader.SetAsFirstSibling();

        savegamesUIManager.UpdateLoadGameButtonState();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if(!AreYouSureObject.gameObject.activeSelf && !OptionsObjectQuality.gameObject.activeSelf && !CreditsObject.gameObject.activeSelf && !OptionsObject.gameObject.activeSelf  && !LoadGameMenuObject.gameObject.activeSelf && !OptionsObjectBrightness.gameObject.activeSelf && !OptionsObjectAudio.gameObject.activeSelf && !OptionsObjectDisplay.gameObject.activeSelf)
            {
                TryQuit();
            }
            else
            {
                if (CreditsObject.gameObject.activeSelf)
                {
                    CloseCredits();
                }
                else if (OptionsObject.gameObject.activeSelf)
                {
                    CloseOptions();
                }
                else if (AreYouSureObject.gameObject.activeSelf)
                {
                    CancelQuit();
                }
                else if (LoadGameMenuObject.gameObject.activeSelf)
                {
                    CloseLoadGameScreen();
                }
                else if (OptionsObjectBrightness.gameObject.activeSelf) 
                {
                    CloseOptionsBrightness();
                }
                else if (OptionsObjectAudio.gameObject.activeSelf)
                {
                    CloseOptionsAudio();
                }
                else if (OptionsObjectQuality.gameObject.activeSelf)
                {
                    CloseOptionsQuality();
                }
                else if (OptionsObjectDisplay.gameObject.activeSelf)
                {
                    CloseOptionsDisplay();
                }
            }
        }
    }

    public void StartNewGame()
    {
        loadingIdicator.Play();
        ManagerSound.ClickSound();

        PersistentInformation.isNewGame = string.IsNullOrEmpty(PersistentInformation.targetSaveLoad) || string.IsNullOrWhiteSpace(PersistentInformation.targetSaveLoad) ? true : false;

        StartCoroutine(StartNewGameCoroutine());
    }

    public void OpenLoadGameScreen()
    {
        MainMenuObject.gameObject.SetActive(false);
        LoadGameMenuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound();
    }

    public void CloseLoadGameScreen()
    {
        LoadGameMenuObject.gameObject.SetActive(false);
        MainMenuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void LoadGame(string loadPath)
    {
        LoadGameMenuObject.gameObject.SetActive(false);
        ManagerSound.ClickSound();
        PersistentInformation.targetSaveLoad = loadPath;
        StartNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TryQuit()
    {
        AreYouSureObject.gameObject.SetActive(true);
        MainMenuObject.gameObject.SetActive(false);
        ManagerSound.ClickSound();
    }

    public void CancelQuit()
    {
        AreYouSureObject.gameObject.SetActive(false);
        MainMenuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void ShowCredits()
    {
        CreditsObject.gameObject.SetActive(true);
        MainMenuObject.gameObject.SetActive(false);
        ManagerSound.ClickSound();
    }

    public void CloseCredits()
    {
        CreditsObject.gameObject.SetActive(false);
        MainMenuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void ShowOptions()
    {
        OptionsObject.gameObject.SetActive(true);
        MainMenuObject.gameObject.SetActive(false);
        ManagerSound.ClickSound();
    }

    public void CloseOptions()
    {
        OptionsObject.gameObject.SetActive(false);
        MainMenuObject.gameObject.SetActive(true);
        ManagerSound.ClickSound2();
    }

    public void ShowOptionsAudio()
    {
        OptionsObject.gameObject.SetActive(false);
        OptionsObjectAudio.gameObject.SetActive(true);
        ManagerSound.ClickSound();
    }

    public void CloseOptionsAudio()
    {
        OptionsObject.gameObject.SetActive(true);
        OptionsObjectAudio.gameObject.SetActive(false);
        ManagerSound.ClickSound2();
    }

    public void ShowOptionsDisplay()
    {
        OptionsObject.gameObject.SetActive(false);
        OptionsObjectDisplay.gameObject.SetActive(true);
        ManagerSound.ClickSound();
    }

    public void CloseOptionsDisplay()
    {
        OptionsObject.gameObject.SetActive(true);
        OptionsObjectDisplay.gameObject.SetActive(false);
        ManagerSound.ClickSound2();
    }

    public void ShowOptionsQuality()
    {
        OptionsObject.gameObject.SetActive(false);
        OptionsObjectQuality.gameObject.SetActive(true);
        ManagerSound.ClickSound();
    }

    public void CloseOptionsQuality()
    {
        OptionsObject.gameObject.SetActive(true);
        OptionsObjectQuality.gameObject.SetActive(false);
        ManagerSound.ClickSound2();
    }

    public void ShowOptionsBrightness()
    {
        OptionsObject.gameObject.SetActive(false);
        OptionsObjectBrightness.gameObject.SetActive(true);
        ManagerSound.ClickSound();
    }

    public void CloseOptionsBrightness()
    {
        OptionsObject.gameObject.SetActive(true);
        OptionsObjectBrightness.gameObject.SetActive(false);
        ManagerSound.ClickSound2();
    }

    public void StartNewGameAnimation()
    {
        startNewGameAnimation.Play();
    }

    private IEnumerator StartNewGameCoroutine()
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //transform.GetComponent<Text>().text = (asyncLoad.progress * 100) + "%";

            if (asyncLoad.progress >= 0.9f)
            {
                loadingText.enabled = false;
                loadingIcon.enabled = false;
                loadingCompleteText.enabled = true;
                loadingCompleteText.gameObject.GetComponent<Animation>().Play();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public void SaveConfiguration()
    {
        ManagerSound.ClickSound();

        string _data =
            "Resolution:" + UISettings.resolutionsContainer.currentOptionText + "\n" +
            "FOV:" + UISettings.FOVSlider.value + "\n" +
            "ScreenMode:" + UISettings.screenModeContainer.currentOptionText + "\n" +
            "UseVsync:" + UISettings.vsyncContainer.currentOptionText + "\n" +
            "Subtitles:" + UISettings.subtitlesContainer.currentOptionText + "\n" +
            "Language:" + UISettings.languagesContainer.currentOptionText + "\n" +
            "MasterVolume:" + UISettings.masterVolumeSlider.value + "\n" +
            "AmbienceVolme:" + UISettings.ambienceVolumeSlider.value + "\n" +
            "MusicVolume:" + UISettings.musicVolumeSlider.value + "\n" +
            "VoiceVolume:" + UISettings.voiceVolumeSlider.value + "\n" +
            "UIVolume:" + UISettings.uiVolumeSlider.value + "\n" +
            "Brightness:" + UISettings.brightnessSlider.value + "\n" +
            "QualityPreset:" + UISettings.presetsContainer.currentOptionText + "\n" +
            "Shadows:" + UISettings.shadowsContainer.currentOptionText + "\n" +
            "Antialiasing:" + UISettings.antialiasingContainer.currentOptionText + "\n" +
            "Textures:" + UISettings.texturesContainer.currentOptionText + "\n" +
            "Particles:" + UISettings.particlesContainer.currentOptionText + "\n" +
            "DrawDistance:" + UISettings.drawDistanceContainer.currentOptionText + "\n" +
            "AmbientOcclusion:" + UISettings.ambientOcclusionContainer.currentOptionText + "\n" +
            "Grass:" + UISettings.grassContainer.currentOptionText;


        CreateConfigFile(_data);

        configurationsInfo[0] = UISettings.resolutionsContainer.currentOptionText;
        configurationsInfo[1] = UISettings.FOVSlider.value.ToString();
        configurationsInfo[2] = UISettings.screenModeContainer.currentOptionText;
        configurationsInfo[3] = UISettings.vsyncContainer.currentOptionText;
        configurationsInfo[4] = UISettings.subtitlesContainer.currentOptionText;
        configurationsInfo[5] = UISettings.languagesContainer.currentOptionText;
        configurationsInfo[6] = UISettings.masterVolumeSlider.value.ToString();
        configurationsInfo[7] = UISettings.ambienceVolumeSlider.value.ToString();
        configurationsInfo[8] = UISettings.musicVolumeSlider.value.ToString();
        configurationsInfo[9] = UISettings.voiceVolumeSlider.value.ToString();
        configurationsInfo[10] = UISettings.uiVolumeSlider.value.ToString();
        configurationsInfo[11] = UISettings.brightnessSlider.value.ToString();
        configurationsInfo[12] = UISettings.presetsContainer.currentOptionText;
        configurationsInfo[13] = UISettings.shadowsContainer.currentOptionText;
        configurationsInfo[14] = UISettings.antialiasingContainer.currentOptionText;
        configurationsInfo[15] = UISettings.texturesContainer.currentOptionText;
        configurationsInfo[16] = UISettings.particlesContainer.currentOptionText;
        configurationsInfo[17] = UISettings.drawDistanceContainer.currentOptionText;
        configurationsInfo[18] = UISettings.ambientOcclusionContainer.currentOptionText;
        configurationsInfo[19] = UISettings.grassContainer.currentOptionText;

        ApplySettings();
    }

    public void CreateConfigFile(string settingsData)
    {
        StreamWriter writer;
        FileInfo t = new FileInfo(PersistentInformation.defaultSettingsLocation + "//" + PersistentInformation.defaultSettingsName);
        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }

        writer.Write(settingsData);
        writer.Close();
    }

    private IEnumerator WaitForConfigCreation()
    {
        while (!new FileInfo(PersistentInformation.defaultSettingsLocation + "//" + PersistentInformation.defaultSettingsName).Exists)
        {
            yield return null;
        }

        LoadConfiguration();

        ApplySettings();
    }

    private void LoadConfiguration()
    {
        // Get the default path to save settings

        string configPath = PersistentInformation.defaultSettingsLocation + "//" + PersistentInformation.defaultSettingsName;

        // Check if it exits, if no create a config file with default settings , wait for it create and load again

        if(!new FileInfo(configPath).Exists)
        {
            CreateConfigFile(PersistentInformation.defaultSettingsFileInformation);

            StartCoroutine(WaitForConfigCreation());

            return;
        }

        // Read all text saved on the settings file , split it into separated info, and set the string array of configurationsInfo the separated infos

        StreamReader r = File.OpenText(configPath);
        string _info = r.ReadToEnd();
        r.Close();
        configurationsInfo = _info.Split('\n');

        // Remove the names and leave only the values

        configurationsInfo[0] = configurationsInfo[0].Remove(0, 11);
        configurationsInfo[1] = configurationsInfo[1].Remove(0, 4);
        configurationsInfo[2] = configurationsInfo[2].Remove(0, 11);
        configurationsInfo[3] = configurationsInfo[3].Remove(0, 9);
        configurationsInfo[4] = configurationsInfo[4].Remove(0, 10);
        configurationsInfo[5] = configurationsInfo[5].Remove(0, 9);
        configurationsInfo[6] = configurationsInfo[6].Remove(0, 13);
        configurationsInfo[7] = configurationsInfo[7].Remove(0, 14);
        configurationsInfo[8] = configurationsInfo[8].Remove(0, 12);
        configurationsInfo[9] = configurationsInfo[9].Remove(0, 12);
        configurationsInfo[10] = configurationsInfo[10].Remove(0, 9);
        configurationsInfo[11] = configurationsInfo[11].Remove(0, 11);
        configurationsInfo[12] = configurationsInfo[12].Remove(0, 14);
        configurationsInfo[13] = configurationsInfo[13].Remove(0, 8);
        configurationsInfo[14] = configurationsInfo[14].Remove(0, 13);
        configurationsInfo[15] = configurationsInfo[15].Remove(0, 9);
        configurationsInfo[16] = configurationsInfo[16].Remove(0, 10);
        configurationsInfo[17] = configurationsInfo[17].Remove(0, 13);
        configurationsInfo[18] = configurationsInfo[18].Remove(0, 17);
        configurationsInfo[19] = configurationsInfo[19].Remove(0, 6);

        // Set the containers and slides the info from the array

        UISettings.resolutionsContainer.SetCurrentOption(configurationsInfo[0]);
        UISettings.FOVSlider.value = float.Parse(configurationsInfo[1]);
        UISettings.screenModeContainer.SetCurrentOption(configurationsInfo[2]);
        UISettings.vsyncContainer.SetCurrentOption(configurationsInfo[3]);
        UISettings.subtitlesContainer.SetCurrentOption(configurationsInfo[4]);
        UISettings.languagesContainer.SetCurrentOption(configurationsInfo[5]);
        UISettings.masterVolumeSlider.value = float.Parse(configurationsInfo[6]);
        UISettings.ambienceVolumeSlider.value = float.Parse(configurationsInfo[7]);
        UISettings.musicVolumeSlider.value = float.Parse(configurationsInfo[8]);
        UISettings.voiceVolumeSlider.value = float.Parse(configurationsInfo[9]);
        UISettings.uiVolumeSlider.value = float.Parse(configurationsInfo[10]);
        UISettings.brightnessSlider.value = float.Parse(configurationsInfo[11]);
        UISettings.presetsContainer.SetCurrentOption(configurationsInfo[12]);
        UISettings.shadowsContainer.SetCurrentOption(configurationsInfo[13]);
        UISettings.antialiasingContainer.SetCurrentOption(configurationsInfo[14]);
        UISettings.texturesContainer.SetCurrentOption(configurationsInfo[15]);
        UISettings.particlesContainer.SetCurrentOption(configurationsInfo[16]);
        UISettings.drawDistanceContainer.SetCurrentOption(configurationsInfo[17]);
        UISettings.ambientOcclusionContainer.SetCurrentOption(configurationsInfo[18]);
        UISettings.grassContainer.SetCurrentOption(configurationsInfo[19]);

        // Disable all the confirm buttons that get enabled when you change some configuration

        foreach (Button bt in UISettings.confirmButtons)
        {
            bt.gameObject.SetActive(false);
        }
    }

    public void ApplySettings()
    {
        string[] res = UISettings.resolutionsContainer.currentOptionText.Split('x');
        int scrWidth = int.Parse(res[0]);
        int scrHeight = int.Parse(res[1]);
        FullScreenMode mode = FullScreenMode.MaximizedWindow;
        switch (UISettings.screenModeContainer.currentOptionText)
        {
            case "Full Screen":
                mode = FullScreenMode.MaximizedWindow;
                break;
            case "Window":
                mode = FullScreenMode.Windowed;
                break;
        }

        Screen.SetResolution(scrWidth, scrHeight, mode);

        string useVsync = UISettings.vsyncContainer.currentOptionText;

        QualitySettings.vSyncCount = useVsync == "On" ? 1 : 0;

        foreach (Button bt in UISettings.confirmButtons)
        {
            bt.gameObject.SetActive(false);
        }
    }

    public void DisableDisplayConfirmButton(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void ChangeQualityPreset(UIOptionsContainer from)
    {
        string value = from.currentOptionText;
    }
}
