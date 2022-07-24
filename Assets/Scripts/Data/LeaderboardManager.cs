using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;



[System.Serializable]
public struct LeaderBoard
{
    public List<string> names;
    public List<int> scores;
    public int personnalHighscore;
}


public class LeaderboardManager : MonoBehaviour
{
    private DataManager dataManager;
    private LoginManager loginManager;


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
                leaderboards[mode, dif].names = new List<string>();
                leaderboards[mode, dif].scores = new List<int>();
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
                    //string[] gt = scores[j].metadata.Split('.');
                    //Vector3Int gtv = new Vector3Int(int.Parse(gt[0]), int.Parse(gt[1]), int.Parse(gt[2]));
                    //leaderboards[gtv.x, gtv.y, gtv.z].names.Add(scores[j].member_id.Split('.')[0]);
                    //leaderboards[gtv.x, gtv.y, gtv.z].scores.Add(scores[j].score);
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
        return (int)gD.gameWheather + "//"
            + OptionsToInt(gD.gameOptions);
    }


    /// <summary>
    /// Return an int being the index for the highscores 3rd arg given the game options
    /// </summary>
    /// <param name="GOs">Game Options list</param>
    /// <returns></returns>
    public int OptionsToInt(List<GameOption> GOs)
    {
        int Result = 0;

        if (GOs.Contains(GameOption.BONUS)) Result += 1000;
        if (GOs.Contains(GameOption.OBSTACLE)) Result += 100;
        if (GOs.Contains(GameOption.OBJECTIF)) Result += 10;
        if (GOs.Contains(GameOption.WEAPONS)) Result += 1;

        return Result;
    }


    private string GameTypeToString(GameData gD)
    {
        return "";
    }
}
