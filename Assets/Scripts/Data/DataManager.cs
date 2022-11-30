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

    public float playerVolume;
    public float ambianceVolume;
    public float enemiesVolume;
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
    public bool backview;

    public JoystickMode joystickMode;
    public float centerSize;
    public float boundarySize;
    public float joystickSize;
    public float joystickPosX;
    public float joystickPosY;
    public float jumpButtonSize;
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

    public bool proDiff;
    public bool starDiff;
    public bool veteranDiff;
    public bool legendDiff;

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
    public int tutoNumber;

    public PlayerInfo player;
    public DefenderAttributesSO enemy;
    public AttackerAttributesSO[] team;
    public GameObject stadium;
    public int parkour;
    public List<GameObject> weapons;
}


[System.Serializable]
public struct PlayerInfo
{
    public Avatar avatar;
    public Mesh mesh;
    public Material[] materials;

    public PlayerAttributesSO attributes;
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
    [HideInInspector] public StatsData[] statsDatas = new StatsData[15];

    // Current game data
    [HideInInspector] public GameData gameData;


    public CardsContainerSO cardsContainer;

    private int onlineFileID;
    private bool onlineFileIDLoaded = false;
    private bool quitting = false;
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

            if (InstanceDataManager.gameData.gameMode == GameMode.TUTORIAL) InstanceDataManager.InitGameData();
            else if (InstanceDataManager.gameData.gameMode == GameMode.DRILL && InstanceDataManager.gameData.gameDrill == GameDrill.PARKOUR)
                InstanceDataManager.gameData.gameWeather = GameWeather.SUN;

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
        if (playerPrefs.enemyIndex.Length != 5)
            playerPrefs.enemyIndex = new int[5];

        if (playerPrefs.playerIndex >= inventoryData.players.Length) playerPrefs.playerIndex = 0;
        for (int i = 0; i < playerPrefs.enemyIndex.Length; i++)
            if (playerPrefs.enemyIndex[i] >= cardsContainer.enemyCards.GetCardsByIndex(i).Count) playerPrefs.enemyIndex[i] = 0;
        if (playerPrefs.stadiumIndex >= inventoryData.stadiums.Length) playerPrefs.stadiumIndex = 0;
        if (playerPrefs.parkourIndex >= cardsContainer.parkourCards.Count) playerPrefs.parkourIndex = 0;
        for (int i = 0; i < playerPrefs.teamIndex.Length; i++)
            if (cardsContainer.teamCards.GetAttacker(playerPrefs.teamIndex[i]) == null) playerPrefs.teamIndex[i] = 1; // Playerprefs team corresponds to the attacker number
    }

    private void ResetPlayerPrefs()
    {
        playerPrefs.infoButtonsOn = true;

        playerPrefs.teamIndex = new int[] { 1,1,1,1,1 };
        playerPrefs.enemyIndex = new int[5];
        playerPrefs.playerIndex = 0;
        playerPrefs.stadiumIndex = 0;
        playerPrefs.parkourIndex = 0;
    }

    private void InitProgression()
    {
        progressionData.teamMode = true;
        progressionData.zombieMode = true;

        progressionData.proDiff = true;
        progressionData.starDiff = true;
        progressionData.veteranDiff = true;
        progressionData.legendDiff = true;

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
        gameData.enemy = cardsContainer.enemyCards.rookieEnemyCards[playerPrefs.enemyIndex[0]].attribute;

        gameData.team = new AttackerAttributesSO[5];
        for (int i = 0; i < gameData.team.Length; i++)
            gameData.team[i] = cardsContainer.teamCards.GetAttacker(playerPrefs.teamIndex[i]).attribute;
    }

    private void InitStatsDatas()
    {
        statsDatas = new StatsData[15];
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

        audioData.playerVolume = 1;
        audioData.enemiesVolume = 1;
        audioData.ambianceVolume = 1;
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

        gameplayData.joystickMode = JoystickMode.FIXED;
        gameplayData.centerSize = 75;
        gameplayData.boundarySize = 150;
        gameplayData.joystickSize = 75;
        gameplayData.joystickPosX = 400;
        gameplayData.joystickPosY = 300;
        gameplayData.jumpButtonSize = 250;
    }

    private void FixGameplay()
    {
        if (gameplayData.centerSize == 0) gameplayData.centerSize = 75;
        if (gameplayData.boundarySize == 0) gameplayData.boundarySize = 150;
        if (gameplayData.joystickSize == 0) gameplayData.joystickSize = 75;
        if (gameplayData.joystickPosX == 0) gameplayData.joystickPosX = 400;
        if (gameplayData.joystickPosY == 0) gameplayData.joystickPosY = 300;
        if (gameplayData.jumpButtonSize == 0) gameplayData.jumpButtonSize = 250;
    }

    public void ClearGameData()
    {
        gameData.gameMode = GameMode.DEFENDERS;
        gameData.gameDifficulty = GameDifficulty.ROOKIE;
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
        ResetPlayerPrefs();
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

    public void SaveDatas(bool reset)
    {
        StartCoroutine(SavePlayerData(reset));
    }

    private IEnumerator SavePlayerData(bool reload)
    {
        SaveOnDisk();

        Debug.Log("saved on disk");
        yield return StartCoroutine(SaveOnlineCR());

        if (reload)
        {
            reloadAll = true;
            SceneManager.LoadScene((int)SceneNumber.MENU);
        }

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
            yield return GetOnlineFileID();
            
            LootLockerSDKManager.UploadPlayerFile(Application.persistentDataPath + "/savefile.json", "save", (response) =>
            {
                if (response.success)
                {
                    //Debug.Log("File uploaded successfully : " + response.id);
                    int formerID = OnlineFileID;
                    //Debug.Log("deleting file " + formerID);

                    LootLockerSDKManager.DeletePlayerFile(formerID, (response) =>
                    {
                        //if (response.success)
                        //    Debug.Log("Deleted file successfully");
                        //else
                        //    Debug.Log("File not deleted : " + formerID + ";" + response.text);

                        done = true;
                    });

                    OnlineFileID = response.id;
                }
                else
                {
                    done = true;
                }
            });

            yield return new WaitUntil(() => done);
            yield return new WaitUntil(() => onlineFileIDLoaded);
        }
    }

    public void QuitGame()
    {
        if (!quitting)
        {
            quitting = true;
            StartCoroutine(Quit());
        }
    }

    private IEnumerator Quit()
    {
        yield return StartCoroutine(SavePlayerData(false));

#if (UNITY_EDITOR)
        EditorApplication.ExitPlaymode();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }

    // # LOAD #

    public void LoadData()
    {
        Debug.Log("-----LOAD DATA-----");

        LoadDataFromDisk();

        //Debug.Log("Load managers");
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
            if (ConnectionManager.SessionConnected)
            {
                yield return GetOnlineFileID();

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
        //Debug.Log("Load from disk");
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LoadJSON(json);
        }
        else
        {
            Debug.Log("doesn't exist");
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
        FixGameplay();
    }

    private void Load(bool state)
    {
        loadPopup.SetActive(state);
    }

    // C:/Users/tomnd/AppData/LocalLow/DefaultCompany/DFL 2/
}
