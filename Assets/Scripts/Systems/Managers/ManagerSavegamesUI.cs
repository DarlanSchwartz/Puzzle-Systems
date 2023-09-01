using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerSavegamesUI : MonoBehaviour
{
    public ManagerSave saveManager;
    public Button loadGameButton;
    [Space(5)]
    [Header("Save & Load slots")]
    [Space(5)]
    public List<UIElementSavegame> loadgameSaveSlots;
    [Space(5)]
    public List<UIElementSavegame> savegameSaveSlots;
   
   

    #region Singleton

    private static ManagerSavegamesUI instance;

    public static ManagerSavegamesUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ManagerSavegamesUI>();
            }

            return instance;
        }
    }

    #endregion

    private void Start()
    {
        PersistentInformation.defaultSavegamePath = Application.dataPath + "/Savegames";
    }

    public void UpdateLoadGameButtonState()
    {
        loadGameButton.interactable = loadgameSaveSlots.Count > 0 ? true : false;
    }
}
