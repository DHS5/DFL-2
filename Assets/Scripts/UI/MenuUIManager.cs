using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    private MenuMainManager main;

    private DataManager DataManager
    {
        get { return main.DataManager; }
    }

    private List<GameOption> defenderOptions = new List<GameOption>();
    private List<GameOption> zombieOptions = new List<GameOption>();


    [SerializeField] private GameObject infoButtons;



    // ### Properties ###

    public bool InfoButtonsOn
    {
        set { infoButtons.SetActive(value); }
    }

    public int GameMode
    {
        set { DataManager.gameData.gameMode = (GameMode) value; }
    }
    public int GameDifficulty
    {
        set { DataManager.gameData.gameDifficulty = (GameDifficulty) value; }
    }
    public int GameWheather
    {
        set { DataManager.gameData.gameWeather = (GameWeather) value; }
    }
    public int GameDrill
    {
        set { DataManager.gameData.gameDrill = (GameDrill) value; }
    }




    // ### Functions ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }

    private void Start()
    {
        main.SettingsManager.GetManagers();

        ActuData();
    }



    private void ActuData()
    {
        InfoButtonsOn = main.SettingsManager.InfoButtonsOn;
    }

    /// <summary>
    /// Removes or adds a game option
    /// </summary>
    /// <param name="b">True --> Add / False --> Remove</param>
    /// <param name="option">Game option to add/remove</param>
    public void ChooseOption(int option)
    {
        List<GameOption> gameOptions = (DataManager.gameData.gameMode == global::GameMode.ZOMBIE) ? zombieOptions : defenderOptions;

        if (!gameOptions.Contains((GameOption)option)) { gameOptions.Add((GameOption)option); }
        else { gameOptions.Remove((GameOption)option); }

        DataManager.gameData.gameOptions = new List<GameOption>(gameOptions);
    }

    public void ActuOptions()
    {
        DataManager.gameData.gameOptions = (DataManager.gameData.gameMode == global::GameMode.ZOMBIE) ? zombieOptions : defenderOptions;
    }
}
