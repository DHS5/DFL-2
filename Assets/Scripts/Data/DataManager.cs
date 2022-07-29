using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;



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

    public bool rainWeather;
    public bool fogWeather;

    public bool bonusOpt;
    public bool obstacleOpt;
    public bool objectifOpt;
    public bool weaponOpt;
}

[System.Serializable]
public struct StatsData // One for each mode-diff
{
    public int gameNumber;
    public int totalScore;
    public int[] wavesReached;
}

[System.Serializable]
public struct GameData
{
    public GameMode gameMode;
    public GameDifficulty gameDifficulty;
    public GameWeather gameWeather;
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

    
    private LoginManager loginManager;

    private int onlineFileID = 50355;

    // Online Savable Data
    [HideInInspector] public AudioData audioData;
    [HideInInspector] public PlayerPrefs playerPrefs;
    [HideInInspector] public GameplayData gameplayData;
    [HideInInspector] public PlayerData playerData;
    [HideInInspector] public InventoryData inventoryData;
    [HideInInspector] public ProgressionData progressionData;
    [HideInInspector] public StatsData[] statsDatas = new StatsData[9];

    // Current game data
    [HideInInspector] public GameData gameData;


    public CardsContainerSO cardsContainer;


    private bool datasLoaded;


    // ### Properties ###

    private bool Connected
    {
        get { return loginManager.State == ConnectionState.CONNECTED || loginManager.State == ConnectionState.GUEST; }
    }

    private int OnlineFileID
    {
        get { return onlineFileID; }
        set
        {
            //onlineFileID = value;
            LootLockerSDKManager.UpdateOrCreateKeyValue("OnlineFileID", value.ToString(), (response) => { });
        }
    }


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

        loginManager = FindObjectOfType<LoginManager>();

        // Load the personnal highscores and player preferences
        LoadPlayerData();

        // Clear the game data (modes etc... for the first game)
        ClearGameData();

        // Initialize favorite player and stadium indexes
        InitGameData();
    }


    // ### Functions ###

    public IEnumerator LoadDatas()
    {
        datasLoaded = false;
        LoadPlayerData();
        yield return new WaitUntil(() => datasLoaded);
    }

    public void GetOnlineFileID()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage("OnlineFileID", (response) =>
        {
            if (response.success)
                if (response.payload != null)
                    onlineFileID = int.Parse(response.payload.value);
            else
                Debug.Log("Couldn't get online file ID");
        });
    }

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

    private void InitProgression()
    {
        progressionData.teamMode = true;
        progressionData.zombieMode = true;

        progressionData.normalDiff = true;
        progressionData.hardDiff = true;

        progressionData.rainWeather = true;
        progressionData.fogWeather = true;

        progressionData.bonusOpt = true;
        progressionData.objectifOpt = true;
        progressionData.obstacleOpt = true;
        progressionData.weaponOpt = true;
    }

    private void InitInventory()
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

    private void InitStatsDatas()
    {
        statsDatas = new StatsData[9];
        for (int i = 0; i < statsDatas.Length; i++)
            statsDatas[i].wavesReached = new int[1] {0};
    }


    public void ClearGameData()
    {
        gameData.gameMode = GameMode.DEFENDERS;
        gameData.gameDifficulty = GameDifficulty.EASY;
        gameData.gameWeather = GameWeather.SUN;
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

        public StatsData[] statsDatas;
    }


    public void SavePlayerData()
    {
        SaveData data = new();

        data.audioData = audioData;
        data.gameplayData = gameplayData;
        data.playerPrefs = playerPrefs;
        data.inventoryData = inventoryData;
        data.progressionData = progressionData;
        data.statsDatas = statsDatas;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        //if (Connected)
        //{
        //    LootLockerSDKManager.DeletePlayerFile(OnlineFileID, (response) =>
        //    {
        //        if (response.success)
        //            Debug.Log("Deleted file successfully");
        //        else
        //            Debug.Log("File not deleted : " + onlineFileID + ";" + response.text);
        //    
        //        LootLockerSDKManager.UploadPlayerFile(Application.persistentDataPath + "/savefile.json", "save", (response) =>
        //        {
        //            if (response.success)
        //            {
        //                Debug.Log("File uploaded successfully");
        //                onlineFileID = response.id;
        //                Debug.Log(onlineFileID + " / " + response.id);
        //                OnlineFileID = response.id;
        //            }
        //        });
        //    });
        //}

        //Debug.Log(Application.persistentDataPath + "/savefile.json");
    }

    /// <summary>
    /// Load the game record from the corresponding file
    /// </summary>
    public void LoadPlayerData()
    {
        Debug.Log("-----LOAD DATA-----");
        InitInventory();
        InitProgression();
        InitStatsDatas();

        string path = "";

        if (Connected)
        {
            LootLockerSDKManager.GetPlayerFile(OnlineFileID, (response) =>
            {
                if (response.success)
                {
                    path = response.url;
                    StartCoroutine(LoadJSONFromURL(path));
                }
            });
        }
        if (datasLoaded) return;

        LoadDataFromDisk();        

        InitPlayerPrefs();

        datasLoaded = true;
    }

    private IEnumerator LoadJSONFromURL(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.error != "")
        {
            Debug.Log("Loaded file successfully");

            string json = request.downloadHandler.text;

            LoadJSON(json);

            datasLoaded = true;
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    private void LoadDataFromDisk()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoadJSON(json);
        }
    }

    private void LoadJSON(string json)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        audioData = data.audioData;
        gameplayData = data.gameplayData;
        playerPrefs = data.playerPrefs;
        inventoryData = data.inventoryData;
        progressionData = data.progressionData;
        statsDatas = data.statsDatas;
    }

    // C:/Users/tomnd/AppData/LocalLow/DefaultCompany/DFL 2/
}
