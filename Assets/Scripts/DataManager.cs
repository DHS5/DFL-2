using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;


public struct LeaderBoard
{
    public List<string> names;
    public List<int> scores;
    public int personnalHighscore;
}

public struct AudioData
{
    public bool soundOn;
    public float soundVolume;

    public bool musicOn;
    public float musicVolume;
    public int musicNumber;

    public bool loopOn;
}

public struct GameplayData
{
    public float yms;
    public float ysr;
    public bool fps;
}

public struct AuthentificationData
{
    public int id;
    public string name;
}

public struct ProgressData
{
    public int coins;
    public string[] playerSkins;
    public string[] stadiumSkins;
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


    [Tooltip("Game Mode to pass to the GameManager")]
    [HideInInspector] public GameMode gameMode;

    [Tooltip("Difficulty to pass to the GameManager")]
    [HideInInspector] public GameDifficulty difficulty;

    [Tooltip("Game options to pass to the GameManager")]
    [HideInInspector] public List<GameOption> options = new List<GameOption>();

    readonly public int leaderboardLimit = 10;

    [HideInInspector] public LeaderBoard[,,] leaderboards = new LeaderBoard[4, 3, 8];

    readonly int[] lb_ID = { 2909, 2911, 2912, 2913 };

    [HideInInspector] public string highName;
    [HideInInspector] public int highWave;
    [HideInInspector] public int highIndex;



    [HideInInspector] public float yMouseSensitivity;
    [HideInInspector] public float ySmoothRotation;

    [HideInInspector] public AudioData audioData;

    [HideInInspector] public bool musicOn;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public int musicNumber;
    [HideInInspector] public float musicTime;

    [HideInInspector] public bool soundOn;
    [HideInInspector] public float soundVolume;

    [HideInInspector] public bool loopOn;

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void JS_FileSystem_Sync();
#endif


    /// <summary>
    /// Gets the Singleton Instance
    /// </summary>
    private void Awake()
    {
        if (InstanceDataManager != null)
        {
            Destroy(this);
            // Clears the options when starting the menu
            InstanceDataManager.options.Clear();
            return;
        }
        InstanceDataManager = this;
        DontDestroyOnLoad(gameObject);

        // Load the personnal highscores and player preferences
        LoadPlayerData();

        // Starts a LootLocker session and load the leaderboards
        StartSession();
    }


    /// <summary>
    /// Starts a LootLocker session and load the leaderboards if session successfully
    /// </summary>
    private void StartSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("success");
                LoadLeaderboards();
            }
            else Debug.Log("failed");
        });
    }


    private string GametypeToMeta(GameMode GM, GameDifficulty GD, List<GameOption> GOs)
    {
        return (int) GM - 1 + "." + (int) GD / 2 + "." + OptionsToInt(GOs);
    }


    public void PostScore(GameMode GM, GameDifficulty GD, List<GameOption> GOs)
    {
        Debug.Log("gt meta : " + GametypeToMeta(GM, GD, GOs));
        LootLockerSDKManager.SubmitScore(highName + "." + GametypeToMeta(GM, GD, GOs), highWave, lb_ID[(int) GM - 1], GametypeToMeta(GM, GD, GOs), (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully submitted");
                LoadLeaderboards();
            }
            else Debug.Log("Failed to submit");
        });
    }


    private void ClearLeaderboards(int mode)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                leaderboards[mode, i, j].names = new List<string>();
                leaderboards[mode, i, j].scores = new List<int>();
            }
        }
    }



    public void LoadLeaderboards()
    {
        for (int i = 0; i < 4; i++)
        {
            LoadLeaderboard(i);
        }
    }


    private void LoadLeaderboard(int mode)
    {
        ClearLeaderboards(mode);

        LootLockerSDKManager.GetScoreList(lb_ID[mode], 100, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int j = 0; j < scores.Length; j++)
                {
                    string[] gt = scores[j].metadata.Split('.');
                    Vector3Int gtv = new Vector3Int(int.Parse(gt[0]), int.Parse(gt[1]), int.Parse(gt[2]));
                    leaderboards[gtv.x, gtv.y, gtv.z].names.Add(scores[j].member_id.Split('.')[0]);
                    leaderboards[gtv.x, gtv.y, gtv.z].scores.Add(scores[j].score);
                }

                Debug.Log("Successfully loaded");
            }
            else Debug.Log("Failed to load");
        });
    }



    /// <summary>
    /// Return an int being the index for the highscores 3rd arg given the game options
    /// </summary>
    /// <param name="GOs">Game Options list</param>
    /// <returns></returns>
    public int OptionsToInt(List<GameOption> GOs)
    {
        if (GOs.Count == 0) return 0;
        if (GOs.Count == 1) return (int)GOs[0] + 1;
        if (GOs.Count == 3) return 7;
        else
        {
            if (GOs.Contains(GameOption.BONUS) && GOs.Contains(GameOption.OBSTACLE)) return 4;
            if (GOs.Contains(GameOption.BONUS) && GOs.Contains(GameOption.FOG)) return 5;
            if (GOs.Contains(GameOption.FOG) && GOs.Contains(GameOption.OBSTACLE)) return 6;
        }
        return 0;
    }



    /// <summary>
    /// Add a score in a list
    /// </summary>
    /// <param name="list">List of scores</param>
    /// <param name="index">Index at which add the score</param>
    /// <param name="value">Score to add</param>
    private void AddScore<T>(T[] list, int index, T value)
    {
        T tmp;
        for (int i = 0; i < 4 - index; i++)
        {
            tmp = list[index + i];
            list[index + i] = value;
            value = tmp;
        }
    }



    /// <summary>
    /// Enters a new score in the highscores if it's in the top 5
    /// </summary>
    /// <param name="GM">Game Mode</param>
    /// <param name="GD">Game Difficulty</param>
    /// <param name="GOs">Game Options list</param>
    /// <param name="name">Name of the player</param>
    /// <param name="wave">Wave reached by the player</param>
    /// <returns>1 if it's a highscore, else 0</returns>
    public bool IsNewHighscoreO(GameMode GM, GameDifficulty GD, List<GameOption> GOs, int wave)
    {
        //int j = 0;
        //while (j < leaderboards[(int)GM - 1, (int)GD / 2, OptionsToInt(GOs)].scores.Count && leaderboards[(int)GM - 1, (int)GD / 2, OptionsToInt(GOs)].names[j] != highName && highName != "Anonym")
        
        
        for (int i = 0; i < leaderboardLimit; i++)
        {
            Debug.Log((int)GM - 1 + "," + (int)GD / 2 + "," + OptionsToInt(GOs) + "," + i);
            if (leaderboards[(int)GM - 1, (int)GD / 2, OptionsToInt(GOs)].scores.Count < leaderboardLimit || leaderboards[(int)GM - 1, (int)GD / 2, OptionsToInt(GOs)].scores[i] < wave)
            {
                return true;
            }
        }
        return false;
    }





    /// <summary>
    /// Class used to save the best scores and players's informations
    /// </summary>
    [System.Serializable]
    class SaveData
    {
        public string name;

        public float yms;
        public float ysr;

        public bool musicOn;
        public float musicVolume;
        public bool soundOn;
        public float soundVolume;
        public bool loopOn;
    }


    public void SavePlayerData()
    {
        SaveData data = new SaveData();

        data.name = highName;

        data.yms = yMouseSensitivity;
        data.ysr = ySmoothRotation;

        data.musicOn = musicOn;
        data.musicVolume = musicVolume;
        data.soundOn = soundOn;
        data.soundVolume = soundVolume;
        data.loopOn = loopOn;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);


#if UNITY_WEBGL
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            JS_FileSystem_Sync();
#endif
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

            highName = data.name;

            yMouseSensitivity = data.yms;
            ySmoothRotation = data.ysr;

            musicOn = data.musicOn;
            musicVolume = data.musicVolume;
            soundOn = data.soundOn;
            soundVolume = data.soundVolume;
            loopOn = data.loopOn;
        }
    }
}
