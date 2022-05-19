using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private DataManager dataManager;
    
    
    [SerializeField] private GameObject infoButtons;


    // ### Properties ###

    public bool InfoButtonsOn
    {
        set { infoButtons.SetActive(value); }
    }

    public int GameMode
    {
        set { dataManager.gameData.gameMode = (GameMode) value; }
    }
    public int GameDifficulty
    {
        set { dataManager.gameData.gameDifficulty = (GameDifficulty) value; }
    }
    public int GameWheather
    {
        set { dataManager.gameData.gameWheather = (GameWheather) value; }
    }
    public int GameDrill
    {
        set { dataManager.gameData.gameDrill = (GameDrill) value; }
    }



    private void Start()
    {
        settingsManager = SettingsManager.InstanceSettingsManager;
        dataManager = DataManager.InstanceDataManager;

        settingsManager.GetManagers();
    }


    // ### Functions ###

    /// <summary>
    /// Removes or adds a game option
    /// </summary>
    /// <param name="b">True --> Add / False --> Remove</param>
    /// <param name="option">Game option to add/remove</param>
    public void ChooseOption(int option)
    {
        if (!dataManager.gameData.gameOptions.Contains((GameOption)option)) { dataManager.gameData.gameOptions.Add((GameOption)option); }
        else { dataManager.gameData.gameOptions.Remove((GameOption)option); }
    }


    // ### Tools ###

    public void SetAsLastSibling(GameObject go)
    {
        go.transform.SetAsLastSibling();
    }
}
