using UnityEngine;

public static class PersistentInformation
{
    public static string targetSaveLoad = string.Empty;
    public static string targetSaveThumbnail = string.Empty;
    public static int targetSceneIndex = 0;
    public static int savegamesAmount = 5;
    public static bool isNewGame = true;
    public static string defaultSavegamePath = Application.dataPath + "/Savegames";
    public static string defaultSettingsLocation = Application.dataPath;
    public static string defaultSettingsName = "settings.config";
    public static string currentLanguage = "English";
    public static KeyCode quickSaveButton = KeyCode.F5;
    public static string defaultSettingsFileInformation =
            "Resolution:1024x768\n" +
            "FOV:60\n" +
            "ScreenMode:Full Screen\n" +
            "UseVsync:Off\n" +
            "Subtitles:Off\n" +
            "Language:English\n" +
            "MasterVolume:100\n" +
            "AmbienceVolme:100\n" +
            "MusicVolume:100\n" +
            "VoiceVolume:100\n" +
            "UIVolume:100\n" +
            "Brightness:0,2042079\n" +
            "QualityPreset:Custom\n" +
            "Shadows:Very High\n" +
            "Antialiasing:Disabled\n" +
            "Textures:Low\n" +
            "Particles:On\n" +
            "DrawDistance:Very Far\n" +
            "AmbientOcclusion:High\n" +
            "Grass:On";
}
