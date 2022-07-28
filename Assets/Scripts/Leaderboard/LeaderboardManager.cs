using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;



[System.Serializable]
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


    [SerializeField] private Leaderboard[] leaderboards;
    [SerializeField] private LeaderboardRow personnalHighRow;


    private Leaderboard currentLeaderboard;

    readonly int leaderboardLimit = 100;
    readonly int[] leaderboardIDs = { 4969, 4970, 4971, 4972, 4974, 4975, 4976, 4977, 4978 };


    private int mode;
    private int difficulty;


    private bool loaded = false;


    // ### Properties ###

    private Leaderboard CurrentLeaderboard
    {
        get { return currentLeaderboard; }
        set
        {
            if (currentLeaderboard != null) currentLeaderboard.SetActive(false);
            currentLeaderboard = value;
            currentLeaderboard.SetActive(true);
            LoadPersonnalHighscore();
            //LoadLeaderboard();
        }
    }

    private OnlinePlayerInfo PlayerInfo
    {
        get { return loginManager.playerInfo; }
    }
    private bool Connected
    {
        get { return loginManager.State == ConnectionState.CONNECTED || loginManager.State == ConnectionState.GUEST; }
    }

    public int Mode { set { mode = value; CurrentLeaderboard = leaderboards[LeaderboardIndex]; } }
    public int Difficulty { set { difficulty = value; CurrentLeaderboard = leaderboards[LeaderboardIndex]; } }

    public int LeaderboardIndex { get { return mode * 3 + difficulty; } }

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


    public void PostScore(GameData gameData, int score, int wave)
    {
        if (!Connected) return;
        //Debug.Log("gt meta : " + GametypeToMeta(gameData));
        LootLockerSDKManager.SubmitScore(PlayerInfo.id.ToString(), score, leaderboardIDs[GameTypeToLeaderboardIndex(gameData)], GetMeta(gameData, wave),
            (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully submitted");
                    LoadLeaderboard(GameTypeToLeaderboardIndex(gameData), true);
                }
                else Debug.Log("Failed to submit");
            });
    }


    private void LoadPersonnalHighscore()
    {
        personnalHighRow.Item = CurrentLeaderboard.personnalHigh;
    }

    public void LoadLeaderboards()
    {
        if (loaded)
        { 
            for (int i = 0; i < leaderboards.Length; i++)
            {
                LoadLeaderboard(i, false);
            }

            CurrentLeaderboard = leaderboards[0];
            loaded = true;
        }

        else
        {
            for (int i = 0; i < leaderboards.Length; i++)
            {
                LoadLeaderboard(i, true);
            }

            CurrentLeaderboard = leaderboards[LeaderboardIndex];
        }
    }

    public void ClearLeaderboards()
    {
        foreach (var leaderboard in leaderboards)
            leaderboard.Clear();
    }


    private void LoadLeaderboard(int index, bool safe)
    {
        LootLockerSDKManager.GetScoreList(leaderboardIDs[index], leaderboardLimit, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int j = 0; j < scores.Length; j++)
                {
                    string[] meta = MetaToStrings(scores[j].metadata);
                    string pseudo = scores[j].player.name != "" ? scores[j].player.name : scores[j].member_id;
                    LeaderboardItem item = new() { rank = scores[j].rank, name = pseudo, score = scores[j].score, wave = meta[2], wheather = meta[0], options = meta[1] };
                    leaderboards[index].Add(item, safe);

                    if (int.Parse(scores[j].member_id) == PlayerInfo.id)
                        leaderboards[index].personnalHigh = item;
                }

                LoadPersonnalHighscore();
                Debug.Log("Successfully loaded");
            }
            else Debug.Log("Failed to load");
        });
    }

    private void LoadLeaderboard()
    {
        LoadLeaderboard(LeaderboardIndex, true);
    }


    // ### Tools ###

    private string GetMeta(GameData data, int wave)
    {
        return GametypeToMeta(data) + "//" + wave;
    }

    private string GametypeToMeta(GameData gD)
    {
        return gD.gameWeather.ToString() + "//"
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

    private int GameTypeToLeaderboardIndex(GameData gD)
    {
        return (int)gD.gameMode * 3 + (int)gD.gameDifficulty;
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
