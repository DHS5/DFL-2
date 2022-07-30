using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    private MenuMainManager main;

    private DataManager dataManager;

    [SerializeField] private StatBoard[] statBoards;


    private StatsData[] stats = new StatsData[9];


    private StatBoard currentStatBoard;

    private int mode;
    private int difficulty;

    // ### Properties ###

    public StatBoard CurrentStatBoard
    {
        get { return currentStatBoard; }
        set
        {
            if (currentStatBoard != null) currentStatBoard.SetActive = false;
            currentStatBoard = value;
            currentStatBoard.SetActive = true;
        }
    }

    public int Mode { set { mode = value; CurrentStatBoard = statBoards[BoardIndex]; } }
    public int Difficulty { set { difficulty = value; CurrentStatBoard = statBoards[BoardIndex]; } }

    public int BoardIndex { get { return mode * 3 + difficulty; } }


    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }
    private void Start()
    {
        dataManager = main.DataManager;
    }


    public void AddGameToStats(GameData type, int score, int wave)
    {
        int index = (int)type.gameMode * 3 + (int)type.gameDifficulty;

        stats[index].gameNumber++;
        stats[index].totalScore += score;

        int baseSize = stats[index].wavesReached.Length;
        if (baseSize > wave)
        {
            stats[index].wavesReached[wave - 1]++;
        }
        else
        {
            int[] newWavesReached = new int[wave];
            for (int i = 0; i < baseSize; i++)
            {
                newWavesReached[i] = stats[index].wavesReached[i];
            }
            newWavesReached[wave - 1] = 1;

            stats[index].wavesReached = newWavesReached;
        }

        dataManager.statsDatas[index] = stats[index];
        LoadStatBoard(index);
    }



    public void LoadStatsBoards()
    {
        for (int i = 0; i < statBoards.Length; i++)
        {
            LoadStatBoard(i);
        }
        CurrentStatBoard = statBoards[BoardIndex];
    }


    private void LoadStatBoard(int index)
    {
        statBoards[index].Data = dataManager.statsDatas[index];
        stats[index] = dataManager.statsDatas[index];
    }
}
