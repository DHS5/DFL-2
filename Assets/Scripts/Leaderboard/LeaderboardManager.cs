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


    public void PostScore(GameData gameData, int score, int wave)
    {
        Debug.Log("gt meta : " + GametypeToMeta(gameData));
        LootLockerSDKManager.SubmitScore(PlayerInfo.id.ToString(), score, leaderboard_IDs[(int)gameData.gameMode - 1], GetMeta(gameData, wave),
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




    public void LoadLeaderboards()
    {
        for (int i = 0; i < leaderboards.Length; i++)
        {
            LoadLeaderboard(i, false);
        }
    }


    private void LoadLeaderboard(int index, bool safe)
    {
        LootLockerSDKManager.GetScoreList(leaderboard_IDs[index], leaderboardLimit, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int j = 0; j < scores.Length; j++)
                {
                    string[] meta = MetaToStrings(scores[j].metadata);
                    leaderboards[index].Add(
                        new LeaderboardItem() { rank = scores[j].rank, name = scores[j].player.name, score = scores[j].score, wave = meta[2], wheather = meta[0], options = meta[1] },
                        safe);
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
