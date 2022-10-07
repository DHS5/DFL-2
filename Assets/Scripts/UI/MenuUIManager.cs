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


    [Header("UI components")]
    [SerializeField] GameObject[] gameScreenButtons;
    [SerializeField] GameObject[] gameScreenWindows;
    [Space,Space]
    [SerializeField] Toggle[] modeToggles;
    [Space]
    [SerializeField] Toggle[] difficultyToggles;
    [Space]
    [SerializeField] Toggle[] weatherToggles;
    [Space]
    [SerializeField] Toggle[] dTOptionToggles;
    [SerializeField] Toggle[] zOptionToggles;
    [SerializeField] Toggle[] drillOptionToggles;


    // ### Properties ###

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

        ActuGameData(DataManager.gameData);
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

    public void OpenWindow(int i)
    {
        foreach (GameObject g in gameScreenButtons) g.SetActive(true);
        foreach (GameObject g in gameScreenWindows) g.SetActive(false);

        gameScreenButtons[i].SetActive(false);
        gameScreenWindows[i].SetActive(true);
    }

    private void ActuGameData(GameData data)
    {
        for (int i = 0; i < modeToggles.Length; i++)
        {
            if (i == (int)data.gameMode) modeToggles[i].isOn = true;
            else modeToggles[i].isOn = false;
        }
        for (int i = 0; i < difficultyToggles.Length; i++)
        {
            if (i == (int)data.gameDifficulty) difficultyToggles[i].isOn = true;
            else difficultyToggles[i].isOn = false;
        }
        for (int i = 0; i < weatherToggles.Length; i++)
        {
            if (i == (int)data.gameWeather) weatherToggles[i].isOn = true;
            else weatherToggles[i].isOn = false;
        }

        if (data.gameMode == global::GameMode.DEFENDERS || data.gameMode == global::GameMode.TEAM)
        {
            foreach (GameOption go in data.gameOptions) dTOptionToggles[(int)go].isOn = true;
        }
        if (data.gameMode == global::GameMode.ZOMBIE)
        {
            foreach (GameOption go in data.gameOptions) zOptionToggles[(int)go%3].isOn = true;
        }
        if (data.gameMode == global::GameMode.DRILL)
        {
            for (int i = 0; i < drillOptionToggles.Length; i++)
            {
                if (i == (int)data.gameDrill) drillOptionToggles[i].isOn = true;
                else drillOptionToggles[i].isOn = false;
            }
        }
    }
}
