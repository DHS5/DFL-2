using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.SceneManagement;



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
    public int[] enemyIndex;
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
    public int[] parkours;
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
    public int bestScore;
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
    public DefenderAttributesSO enemy;
    public AttackerAttributesSO[] team;
    public GameObject stadium;
    public Parkour parkour;
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

    private MenuMainManager menuMain;

    [SerializeField] private GameObject loadPopup;

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

    private int onlineFileID;
    private bool onlineFileIDLoaded = false;

    private bool reloadAll = false;


    // ### Properties ###

    private MenuMainManager MenuMain
    {
        get
        {
            if (menuMain == null)
                menuMain = FindObjectOfType<MenuMainManager>();
            return menuMain;
        }
    }

    private int OnlineFileID
    {
        get { return onlineFileID; }
        set
        {
            onlineFileID = value;
            onlineFileIDLoaded = false;
            LootLockerSDKManager.UpdateOrCreateKeyValue("OnlineFileID", value.ToString(), 
                (response) => { onlineFileIDLoaded = true; });
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
            if (InstanceDataManager.reloadAll) InstanceDataManager.LoadData();
            else InstanceDataManager.StartCoroutine(InstanceDataManager.LoadMenuManagers());

            InstanceDataManager.ClearGameData();
            return;
        }
        InstanceDataManager = this;
        DontDestroyOnLoad(gameObject);

        // Load the personnal highscores and player preferences
        LoadData();

        // Clear the game data (modes etc... for the first game)
        ClearGameData();

        // Initialize favorite player and stadium indexes
        InitGameData();
    }


    // ### Functions ###

    // ## Datas ##

    private void InitPlayerPrefs()
    {
        if (playerPrefs.teamIndex.Length != 5)
            playerPrefs.teamIndex = new int[5];
        if (playerPrefs.enemyIndex.Length != 3)
            playerPrefs.enemyIndex = new int[3];

        if (playerPrefs.playerIndex >= inventoryData.players.Length) playerPrefs.playerIndex = 0;
        for (int i = 0; i < playerPrefs.enemyIndex.Length; i++)
            if (playerPrefs.enemyIndex[i] >= cardsContainer.enemyCards.GetCardsByIndex(i).Count) playerPrefs.enemyIndex[i] = 0;
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
        // Parkours
        inventoryData.parkours = new int[1] { 1 };
    }

    private void InitGameData()
    {
        //gameData.stadiumIndex = playerPrefs.stadiumIndex;

        gameData.team = new AttackerAttributesSO[5];
    }

    private void InitStatsDatas()
    {
        statsDatas = new StatsData[9];
        for (int i = 0; i < statsDatas.Length; i++)
            statsDatas[i].wavesReached = new int[1] {0};
    }

    private void InitAudio()
    {
        audioData.musicOn = true;
        audioData.musicVolume = 0.5f;
        audioData.musicNumber = 0;

        audioData.soundOn = true;
        audioData.soundVolume = 0.5f;

        audioData.loopOn = true;
    }

    private void InitGameplay()
    {
        gameplayData.viewType = ViewType.TPS;
        gameplayData.tpCameraPos = 0;
        gameplayData.fpCameraPos = 0;

        gameplayData.yms = 3f;
        gameplayData.ysr = 20f;
        gameplayData.headAngle = 10f;

        gameplayData.goalpost = true;
    }


    public void ClearGameData()
    {
        gameData.gameMode = GameMode.DEFENDERS;
        gameData.gameDifficulty = GameDifficulty.EASY;
        gameData.gameWeather = GameWeather.SUN;
        gameData.gameOptions = new List<GameOption>();
        gameData.gameDrill = GameDrill.PRACTICE;
    }

    public void ResetDatas()
    {
        InitAudio();
        InitGameplay();
        InitProgression();
        InitInventory();
        InitStatsDatas();
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


    // # SAVE #

    public void SaveDatas()
    {
        StartCoroutine(SavePlayerData());
    }

    private IEnumerator SavePlayerData()
    {
        SaveOnDisk();

        yield return StartCoroutine(SaveOnlineCR());

        //Debug.Log(Application.persistentDataPath + "/savefile.json");
    }

    private void SaveOnDisk()
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
    }

    private IEnumerator SaveOnlineCR()
    {
        bool done = false;

        if (ConnectionManager.SessionConnected)
        {
            LootLockerSDKManager.UploadPlayerFile(Application.persistentDataPath + "/savefile.json", "save", (response) =>
            {
                if (response.success)
                {
                    Debug.Log("File uploaded successfully");

                    LootLockerSDKManager.DeletePlayerFile(OnlineFileID, (response) =>
                    {
                        if (response.success)
                            Debug.Log("Deleted file successfully");
                        else
                            Debug.Log("File not deleted : " + onlineFileID + ";" + response.text);

                        done = true;
                    });

                    OnlineFileID = response.id;
                }
            });

            yield return new WaitUntil(() => done);
            yield return new WaitUntil(() => onlineFileIDLoaded);
        }
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    private IEnumerator Quit()
    {
        yield return StartCoroutine(SavePlayerData());

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
#else
        Application.Quit();
#endif
    }

    // # LOAD #

    public void LoadData()
    {
        Debug.Log("-----LOAD DATA-----");

        LoadDataFromDisk();

        StartCoroutine(FirstLoad());
        StartCoroutine(LoadMenuManagers());

        reloadAll = false;
    }

    private IEnumerator FirstLoad()
    {
        yield return new WaitUntil(() => MenuMain.awake);

        MenuMain.SettingsManager.Load();
        MenuMain.MusicSource.LoadAudioData(audioData);
    }

    private IEnumerator LoadMenuManagers()
    {
        yield return new WaitUntil(() => MenuMain.awake);

        MenuMain.InventoryManager.ActuInventory();
        MenuMain.StatsManager.LoadStatsBoards();
        MenuMain.ProgressionManager.LoadProgression();
        //MenuMain.LeaderboardManager.LoadLeaderboards();
    }

    public IEnumerator GetOnlineFileID()
    {
        bool gotResponse = false;

        LootLockerSDKManager.GetSingleKeyPersistentStorage("OnlineFileID", (response) =>
        {
            if (response.success)
                if (response.payload != null)
                    onlineFileID = int.Parse(response.payload.value);
                else
                    Debug.Log("Couldn't get online file ID");

            gotResponse = true;
        });
        yield return new WaitUntil(() => gotResponse);
    }

    public void RestoreOnlineData()
    {
        reloadAll = true;

        StartCoroutine(LoadOnlineData());
    }

    /// <summary>
    /// Load the JSON file saved online
    /// </summary>
    private IEnumerator LoadOnlineData()
    {
        Load(true);

        if (ConnectionManager.InternetConnected)
        {
            Debug.Log("Load online data");

            if (ConnectionManager.SessionConnected)
            {
                Debug.Log("Load from online session");
                bool success = false;
                string url = "";
                bool gotResponse = false;

                LootLockerSDKManager.GetPlayerFile(OnlineFileID, (response) =>
                {
                    if (response.success)
                    {
                        success = true;
                        url = response.url;
                    }

                    gotResponse = true;
                });

                yield return new WaitUntil(() => gotResponse);

                if (success)
                    yield return StartCoroutine(LoadJSONFromURL(url));
            }
        }

        Load(false);

        SaveOnDisk();

        SceneManager.LoadScene((int)SceneNumber.MENU);
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
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    private void LoadDataFromDisk()
    {
        Debug.Log("Load from disk");
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoadJSON(json);
        }
        else
        {
            ResetDatas();
            SaveOnDisk();
        }
    }

    private void LoadJSON(string json)
    {
        ResetDatas();

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        audioData = data.audioData;
        gameplayData = data.gameplayData;
        playerPrefs = data.playerPrefs;
        inventoryData = data.inventoryData;
        progressionData = data.progressionData;
        statsDatas = data.statsDatas;

        InitPlayerPrefs();
    }

    private void Load(bool state)
    {
        loadPopup.SetActive(state);
    }

    // C:/Users/tomnd/AppData/LocalLow/DefaultCompany/DFL 2/
}
