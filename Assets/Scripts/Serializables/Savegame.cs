using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

[System.Serializable]public class SaveGame
{
    public string savePath;
    public string thumbnailSavePath; 
    public Sprite thumbnail;
    public string saveTime;
    public string saveName;
    private UserData myData;

    public SaveGame(string path, string thumbnailPath)
    {
        savePath = path;
        thumbnailSavePath = thumbnailPath;

        Texture2D saveThumb = new Texture2D(Screen.width, Screen.height);
        saveThumb.LoadImage(File.ReadAllBytes(thumbnailPath));
        thumbnail = Sprite.Create(saveThumb, new Rect(0, 0, saveThumb.width, saveThumb.height), Vector2.zero);

        StreamReader r = File.OpenText(path);
        string _info = r.ReadToEnd();
        r.Close();

        myData = new UserData();
        myData = (UserData)DeserializeObject(_info);

        string[] allInfo = myData.allObjectsData.Split('%');
        string[] allInfoSplit = allInfo[2].Split(' ');
        string hour = allInfoSplit[1].Remove(5,3);
        saveTime = "Saved in " + allInfoSplit[0] + " at: " + hour + " pm";

        saveName = myData.user.playerName;
    }

    private object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }

    public byte[] StringToUTF8ByteArray(string characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(characters);
        return byteArray;
    }

    public SaveGame(string path)
    {
        savePath = path;
        Texture2D saveThumb = new Texture2D(512, 512);
        thumbnail = Sprite.Create(saveThumb, new Rect(0, 0, saveThumb.width, saveThumb.height), Vector2.zero);
    }
}
