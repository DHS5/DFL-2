using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;



[System.Serializable]
public struct LeaderBoard
{
    public List<LeaderboardItem> rows;
    public LeaderboardItem personnalHigh;
}

public struct LeaderboardItem
{
    public int rank;
    public string name;
    public int score;
    public string wave;
    public string wheather;
    public string options;
}


public class LeaderboardManager : MonoBehaviour
{
    private DataManager dataManager;
    private LoginManager loginManager;


    [SerializeField] private GameObject[] leaderboardContainers;
    [SerializeField] private GameObject leaderboardRowPrefab;



    [HideInInspector] public LeaderBoard[,] leaderboards = new LeaderBoard[3, 3];



    readonly int[] leaderboard_IDs = { 2909, 2911, 2912, 2913 };
    readonly int leaderboardLimit = 100;



    // ### Properties ###

    private OnlinePlayerInfo PlayerInfo
    {
        get { return loginManager.playerInfo; }
    }



    // ### Built-in ###

    private void Awake()
    {
        loginManager = GetComponent<LoginManager>();
    }
    private void Start()
    {
        dataManager = DataManager.InstanceDataManager;
    }


    // ### Functions ###

    private void InstantiateLeaderboardRows()
    {

    }

    public void PostScore(GameData gameData, int score, int wave)
    {
        Debug.Log("gt meta : " + GametypeToMeta(gameData));
        LootLockerSDKManager.SubmitScore(PlayerInfo.id.ToString(), score, leaderboard_IDs[(int)gameData.gameMode - 1], GametypeToMeta(gameData),
            (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully submitted");
                    LoadLeaderboards();
                }
                else Debug.Log("Failed to submit");
            });
    }


    private void ClearLeaderboards()
    {
        for (int mode = 0; mode < 3; mode++)
        {
            for (int dif = 0; dif < 3; dif++)
            {
                leaderboards[mode, dif].rows = new List<LeaderboardItem>();
                leaderboards[mode, dif].personnalHigh = emptyRow();
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
        ClearLeaderboards();

        LootLockerSDKManager.GetScoreList(leaderboard_IDs[mode], leaderboardLimit, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int j = 0; j < scores.Length; j++)
                {
                    //leaderboards[0,0].rows.Add()
                }

                Debug.Log("Successfully loaded");
            }
            else Debug.Log("Failed to load");
        });
    }



    // ### Tools ###

    private string GetMeta(GameData data, int wave)
    {
        return GametypeToMeta(data) + "//" + wave;
    }

    private string GametypeToMeta(GameData gD)
    {
        return gD.gameWheather.ToString() + "//"
            + OptionsToMeta(gD.gameOptions);
    }

    /// <summary>
    /// Return an int being the index for the highscores 3rd arg given the game options
    /// </summary>
    /// <param name="GOs">Game Options list</param>
    /// <returns></returns>
    public string OptionsToMeta(List<GameOption> GOs)
    {
        string result = " ";

        if (GOs.Contains(GameOption.BONUS)) result = "B-";
        if (GOs.Contains(GameOption.WEAPONS)) result = "W-";
        if (GOs.Contains(GameOption.OBSTACLE)) result += "obs-";
        if (GOs.Contains(GameOption.OBJECTIF)) result += "obj";

        return result;
    }

    private string GameTypeToString(GameData gD)
    {
        return "";
    }


    private string[] MetaToStrings(string meta)
    {
        return meta.Split("//");
    }




    private LeaderboardItem emptyRow()
    {
        return new LeaderboardItem() { rank = 0, name = "Empty", score = 0, wave = "0", wheather = "", options = "" };
    }
}
