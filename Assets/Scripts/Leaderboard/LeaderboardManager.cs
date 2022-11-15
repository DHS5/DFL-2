using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private MenuMainManager main;

    [SerializeField] private GameObject waitPopup;

    [SerializeField] private Leaderboard[] leaderboards;
    [SerializeField] private LeaderboardRow personnalHighRow;


    [SerializeField] private GameObject leaderboardLoginButton;
    [SerializeField] private GameObject[] leaderboardObjectsToHide;
    [SerializeField] private RectTransform leaderboardLayout;


    private Leaderboard currentLeaderboard;

    readonly int leaderboardLimit = 100;
    readonly static int[] leaderboardIDs = { 4969, 4970, 4971, 7638, 7639, 4972, 4974, 4975, 7640, 7641, 4976, 4977, 4978, 7642, 7643 };


    private int mode;
    private int difficulty;



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
            LayoutRebuilder.ForceRebuildLayoutImmediate(leaderboardLayout);
        }
    }

    private OnlinePlayerInfo PlayerInfo
    {
        get { return ConnectionManager.playerInfo; }
    }

    public int Mode { set { mode = value; CurrentLeaderboard = leaderboards[LeaderboardIndex]; } }
    public int Difficulty { set { difficulty = value; CurrentLeaderboard = leaderboards[LeaderboardIndex]; } }

    public int LeaderboardIndex { get { return mode * 5 + difficulty; } }

    public bool Loaded { get; private set; } = false;

    // ### Built-in ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }


    // ### Functions ###


    public static void PostScore(GameData gameData, int score, int wave)
    {
        if (!ConnectionManager.SessionConnected) return;
        //Debug.Log("gt meta : " + GametypeToMeta(gameData));
        LootLockerSDKManager.SubmitScore(ConnectionManager.playerInfo.id.ToString(), score, leaderboardIDs[GameTypeToLeaderboardIndex(gameData)], GetMeta(gameData, wave),
            (response) =>
            {
                if (response.success)
                {
                    //Debug.Log("Successfully submitted");
                    //LoadLeaderboard(GameTypeToLeaderboardIndex(gameData), false);
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
        if (ConnectionManager.SessionConnected)
        {
            ShowLeaderboards(true);
            Wait(true);

            if (Loaded)
            {
                ClearLeaderboards();
            }

            for (int i = 0; i < leaderboards.Length; i++)
            {
                LoadLeaderboard(i);
            }

            Loaded = true;

            CurrentLeaderboard = leaderboards[0];
        }
        else
        {
            ShowLeaderboards(false);
        }
    }



    private void LoadLeaderboard(int index)
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
                    leaderboards[index].Add(item);

                    if (int.Parse(scores[j].member_id) == PlayerInfo.id)
                        leaderboards[index].personnalHigh = item;
                }

                LoadPersonnalHighscore();
                //Debug.Log("Successfully loaded");
            }
            else Debug.Log("Failed to load");

            Wait(false);
        });
    }

    private void ClearLeaderboards()
    {
        foreach (var leaderboard in leaderboards)
        {
            leaderboard.Clear();
        }
    }

    private void ShowLeaderboards(bool state)
    {
        foreach (GameObject go in leaderboardObjectsToHide)
            go.SetActive(state);
        leaderboardLoginButton.SetActive(!state);
    }

    // ### Tools ###

    private static string GetMeta(GameData data, int wave)
    {
        return GametypeToMeta(data) + "//" + wave;
    }

    private static string GametypeToMeta(GameData gD)
    {
        return gD.gameWeather.ToString() + "//"
            + OptionsToMeta(gD.gameOptions);
    }

    /// <summary>
    /// Return an int being the index for the highscores 3rd arg given the game options
    /// </summary>
    /// <param name="GOs">Game Options list</param>
    /// <returns></returns>
    public static string OptionsToMeta(List<GameOption> GOs)
    {
        string result = " ";

        if (GOs.Contains(GameOption.BONUS)) result = "B-";
        if (GOs.Contains(GameOption.WEAPONS)) result = "W-";
        if (GOs.Contains(GameOption.OBSTACLE)) result += "obs-";
        if (GOs.Contains(GameOption.OBJECTIF)) result += "obj";

        return result;
    }

    private static int GameTypeToLeaderboardIndex(GameData gD)
    {
        return (int)gD.gameMode * 5 + (int)gD.gameDifficulty;
    }


    private string[] MetaToStrings(string meta)
    {
        return meta.Split("//");
    }




    private void Wait(bool state)
    {
        waitPopup.SetActive(state);
    }
}
