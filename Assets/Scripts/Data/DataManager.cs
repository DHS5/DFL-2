using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using System.IO;



[System.Serializable]
public struct AudioData
{
    public bool soundOn;
    public float soundVolume;

    public bool musicOn;
    public float musicVolume;
    public int musicNumber;

    public bool loopOn;
}


[System.Serializable]
public struct PlayerPrefs
{
    public bool infoButtonsOn;
    public int playerIndex;
    public int stadiumIndex;
    public int enemyIndex;
    public int[] teamIndex;
    public int parkourIndex;
}

[System.Serializable]
public struct GameplayData
{
    public float yms;
    public float ysr;
    public float headAngle;
    public ViewType viewType;
    public int fpCameraPos;
    public int tpCameraPos;
    public bool goalpost;
}

[System.Serializable]
public struct PlayerData
{
    public int id;
    public string name;
}

[System.Serializable]
public struct InventoryData
{
    public int coins;
    public int[] players;
    public int[] stadiums;
    public int[] attackers;
    public int[] weapons;
}

[System.Serializable]
public struct ProgressionData
{
    public bool teamMode;
    public bool zombieMode;

    public bool normalDiff;
    public bool hardDiff;

    public bool rainWheather;
    public bool fogWheather;

    public bool bonusOpt;
    public bool obstacleOpt;
    public bool objectifOpt;
    public bool weaponOpt;
}

[System.Serializable]
public struct GameData
{
    public GameMode gameMode;
    public GameDifficulty gameDifficulty;
    public GameWheather gameWheather;
    public List<GameOption> gameOptions;
    public GameDrill gameDrill;
    public int gameEnemiesRange;

    public GameObject player;
    public int stadiumIndex;
    public GameObject enemy;
    public GameObject[] team;
    public GameObject stadium;
    public GameObject parkour;
    public List<GameObject> weapons;
}


/// <summary>
/// DataManager of the game
/// </summary>
public class DataManager : MonoBehaviour
{
    /// <summary>
    /// Singleton Instance of DataManager
    /// </summary>
    public static DataManager InstanceDataManager { get; private set; }

    


    // Online Savable Data
    [HideInInspector] public AudioData audioData;
    [HideInInspector] public PlayerPrefs playerPrefs;
    [HideInInspector] public GameplayData gameplayData;
    [HideInInspector] public PlayerData playerData;
    [HideInInspector] public InventoryData inventoryData;
    [HideInInspector] public ProgressionData progressionData;

    // Current game data
    [HideInInspector] public GameData gameData;


    public CardsContainerSO cardsContainer;



    /// <summary>
    /// Gets the Singleton Instance
    /// </summary>
    private void Awake()
    {
        if (InstanceDataManager != null)
        {
            Destroy(gameObject);
            // Clears the options when starting the menu
            InstanceDataManager.ClearGameData();
            return;
        }
        InstanceDataManager = this;
        DontDestroyOnLoad(gameObject);

        // Load the personnal highscores and player preferences
        LoadPlayerData();

        // Clear the game data (modes etc... for the first game)
        ClearGameData();

        // Initialize favorite player and stadium indexes
        InitGameData();
    }


    // ### Functions ###

    // ## Data Management ##

    private void InitPlayerPrefs()
    {
        if (playerPrefs.teamIndex == null)
            playerPrefs.teamIndex = new int[5];

        if (playerPrefs.playerIndex >= inventoryData.players.Length) playerPrefs.playerIndex = 0;
        if (playerPrefs.enemyIndex >= cardsContainer.enemyCards.Count) playerPrefs.enemyIndex = 0;
        if (playerPrefs.stadiumIndex >= inventoryData.stadiums.Length) playerPrefs.stadiumIndex = 0;
        if (playerPrefs.parkourIndex >= cardsContainer.parkourCards.Count) playerPrefs.parkourIndex = 0;
        for (int i = 0; i < playerPrefs.teamIndex.Length; i++)
            if (playerPrefs.teamIndex[i] >= inventoryData.attackers.Length) playerPrefs.teamIndex[i] = 0;
    }

    private void ResetProgression()
    {
        progressionData.teamMode = true;
        progressionData.zombieMode = true;

        progressionData.normalDiff = true;
        progressionData.hardDiff = true;

        progressionData.rainWheather = true;
        progressionData.fogWheather = true;

        progressionData.bonusOpt = true;
        progressionData.objectifOpt = true;
        progressionData.obstacleOpt = true;
        progressionData.weaponOpt = true;
    }

    private void ResetInventory()
    {
        // Coins
        inventoryData.coins = 0;
        // Players
        inventoryData.players = new int[1] {1};
        // Stadiums
        inventoryData.stadiums = new int[1] {1};
        // Team
        inventoryData.attackers = new int[4] { 1, 2, 3, 4 };
        // Weapons
        inventoryData.weapons = new int[2] { 1, 2 };
    }

    private void InitGameData()
    {
        gameData.stadiumIndex = playerPrefs.stadiumIndex;

        gameData.team = new GameObject[5];
    }

    public void ClearGameData()
    {
        gameData.gameMode = GameMode.DEFENDERS;
        gameData.gameDifficulty = GameDifficulty.EASY;
        gameData.gameWheather = GameWheather.SUN;
        gameData.gameOptions = new List<GameOption>();
        gameData.gameDrill = GameDrill.PRACTICE;
    }





    /// <summary>
    /// Class used to save the best scores and players's informations
    /// </summary>
    [System.Serializable]
    class SaveData
    {
        public AudioData audioData;

        public GameplayData gameplayData;

        public PlayerPrefs playerPrefs;

        public InventoryData inventoryData;

        public ProgressionData progressionData;
    }


    public void SavePlayerData()
    {
        SaveData data = new();

        data.audioData = audioData;
        data.gameplayData = gameplayData;
        data.playerPrefs = playerPrefs;
        data.inventoryData = inventoryData;
        data.progressionData = progressionData;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        //Debug.Log(Application.persistentDataPath + "/savefile.json");
    }

    /// <summary>
    /// Load the game record from the corresponding file
    /// </summary>
    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            audioData = data.audioData;
            gameplayData = data.gameplayData;
            playerPrefs = data.playerPrefs;
            inventoryData = data.inventoryData;
            progressionData = data.progressionData;
        }

        InitPlayerPrefs();
    }


    // C:/Users/tomnd/AppData/LocalLow/DefaultCompany/DFL 2/
}
