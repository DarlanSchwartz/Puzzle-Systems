using System.IO;
using UnityEngine;

class ApplicationOpenExecute
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnBeforeSplashScreenRuntime()
    {
        PersistentInformation.defaultSavegamePath = Application.dataPath + "/Savegames";
        PersistentInformation.defaultSettingsLocation = Application.dataPath;
        PersistentInformation.defaultSettingsName = "settings.config";
        string[] settingInfo = new string[12];

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        string settingPath = PersistentInformation.defaultSettingsLocation + "\\" + PersistentInformation.defaultSettingsName;
        
        if (!File.Exists(settingPath))
        {
            return;
        }

        StreamReader r = File.OpenText(settingPath);
        string _info = r.ReadToEnd();
        r.Close();
        settingInfo = _info.Split('\n');
        settingInfo[0] = settingInfo[0].Remove(0, 11);
        settingInfo[1] = settingInfo[1].Remove(0, 4);
        settingInfo[2] = settingInfo[2].Remove(0, 11);
        settingInfo[3] = settingInfo[3].Remove(0, 9);
        settingInfo[4] = settingInfo[4].Remove(0, 10);
        settingInfo[5] = settingInfo[5].Remove(0, 9);
        settingInfo[6] = settingInfo[6].Remove(0, 13);
        settingInfo[7] = settingInfo[7].Remove(0, 14);
        settingInfo[8] = settingInfo[8].Remove(0, 12);
        settingInfo[9] = settingInfo[9].Remove(0, 12);
        settingInfo[10] = settingInfo[10].Remove(0, 9);
        settingInfo[11] = settingInfo[11].Remove(0, 11);

        string[] res = settingInfo[0].Split('x');
        int scrWidth = int.Parse(res[0]);
        int scrHeight = int.Parse(res[1]);
        FullScreenMode mode = FullScreenMode.MaximizedWindow;
        switch (settingInfo[2])
        {
            case "Full Screen":
                mode = FullScreenMode.MaximizedWindow;
                break;
            case "Window":
                mode = FullScreenMode.Windowed;
                break;
        }

        Screen.SetResolution(scrWidth, scrHeight, mode);

        string useVsync = settingInfo[3];

        QualitySettings.vSyncCount = useVsync == "On" ? 1 : 0;

    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //static void BeforeSceneLoadRuntime()
    //{
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //}
}
