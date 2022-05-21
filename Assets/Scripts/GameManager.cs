using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


/// <summary>
/// Game Mode
/// </summary>
[System.Serializable] public enum GameMode { DEFENDERS = 0, TEAM = 1, ZOMBIE = 2, DRILL = 3 }

/// <summary>
/// Game Difficulty
/// </summary>
[System.Serializable] public enum GameDifficulty { EASY = 0 , NORMAL = 1 , HARD = 2 }

/// <summary>
/// Game Option
/// </summary>
[System.Serializable] public enum GameOption { BONUS = 0, OBSTACLE = 1, OBJECTIF = 2, NONE = 3, WEAPONS = 4 }

/// <summary>
/// Game Option
/// </summary>
[System.Serializable] public enum GameWheather { SUN = 0, RAIN = 1, FOG = 2 }

/// <summary>
/// Game Option
/// </summary>
[System.Serializable] public enum GameDrill { PRACTICE = 0, ONEVONE = 1, OBJECTIF = 2, PARKOUR = 3 }


/// <summary>
/// Manages the game
/// </summary>
public class GameManager : MonoBehaviour
{
    private MainManager main;


    [HideInInspector] public GameData gameData;


    [Header("Game parameters")]

    [Tooltip("Inspector chosen game mode")]
    public GameMode gameMode;

    [Tooltip("Inspector chosen game difficulty")]
    public GameDifficulty gameDifficulty;

    [Tooltip("Inspector chosen game wheather")]
    public GameWheather gameWheather;

    [Tooltip("Inspector chosen game options")]
    public List<GameOption> gameOptions = new List<GameOption>();

    [Tooltip("Inspector chosen game drill")]
    public GameDrill gameDrill;

    [Tooltip("Range of different enemies that can spawn in one wave")]
    [Range(0, 5)] public int enemiesRange;


    [Tooltip("Current wave number")]
    public int waveNumber;

    [Tooltip("Current score")]
    public int score;



    [Tooltip("Whether the game is running")]
    [HideInInspector] private bool gameOn = false;

    [Tooltip("Whether the game is over")]
    [HideInInspector] private bool gameOver = false;


    // ### Properties ###
    public bool GameOn
    {
        get { return gameOn; }
        set
        {
            if (value == false && gameOn == true)
            {
                gameOn = false;
                PauseGame();
            }
            else if (value == true && gameOn == false)
            {
                UnpauseGame();
            }
        }
    }

    public bool GameOver
    {
        get { return gameOver; }
        set
        {
            if (value == true && gameOver == false)
            {
                gameOver = true;

                
            }
        }
    }


    /// <summary>
    /// Starts the game
    /// </summary>
    private void Start()
    {
        main = MainManager.InstanceMainManager;

        GetGameDatas();

        PrepareGame(true);

        LaunchGame(true);
    }


    /// <summary>
    /// Checks if the game is on and whether the game is over
    /// </summary>
    private void Update()
    {
        // Pause the game on press P
        if (Input.GetKeyDown(KeyCode.P)) PauseGame();
    }


    // ### Functions ###

    /// <summary>
    /// Gets the Game datas from the DataManager
    /// Gets the inspector chosen parameters if the DataManager doesn't exist
    /// </summary>
    private void GetGameDatas()
    {
        if (main.DataManager != null)
        {
            gameData = main.DataManager.gameData;
        }
        else
        {
            gameData.gameMode = gameMode;
            gameData.gameDifficulty = gameDifficulty;
            gameData.gameWheather = gameWheather;
            gameData.gameOptions = gameOptions;
            gameData.gameDrill = gameDrill;
            gameData.gameEnemiesRange = enemiesRange;
        }

        waveNumber = 1;
    }


    /// <summary>
    /// Destroys a field and everything with it before creating the next one
    /// </summary>
    private void CleanGame()
    {
        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            main.TeamManager.ClearAttackers(); // Clear the attackers
        }

        // # Options #
        if (gameData.gameOptions.Contains(GameOption.BONUS))
        {
            main.BonusManager.DestroyBonus(); // Destroys the active bonuses
        }
        if (gameData.gameOptions.Contains(GameOption.OBSTACLE))
        {
            main.ObstacleManager.DestroyObstacles(); // Destroys the active obstacles
        }

        
        main.FieldManager.DestroyField(); // Destroys the former field
    }

    /// <summary>
    /// Generates the new field and everything with it
    /// </summary>
    /// <param name="start">If true generates the first field of the game, if false generates a new field</param>
    private void PrepareGame(bool start)
    {
        // # Essential #
        if (start) main.EnvironmentManager.StartEnvironment(); // Environment
        else main.EnvironmentManager.GenerateEnvironment();
        main.FieldManager.GenerateField(); // Field
        main.EnemiesManager.EnemyWave(); // Enemies


        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
            main.TeamManager.TeamCreation();
        if (gameData.gameMode == GameMode.ZOMBIE)
            main.FieldManager.fieldScript.StopAmbianceAudios(); // A remplacer par une fonction "G�n�rer audio" du game audio manager


        // # Options #
        if (gameData.gameOptions.Contains(GameOption.BONUS))
            main.BonusManager.GenerateBonus();

        if (gameData.gameOptions.Contains(GameOption.OBSTACLE))
            main.ObstacleManager.GenerateObstacles(1); // A remplacer

        if (gameData.gameOptions.Contains(GameOption.OBJECTIF))
            main.ObjectifManager.GenerateObj();

        if (gameData.gameOptions.Contains(GameOption.WEAPONS)) { }
            //main.


        main.GameAudioManager.ActuSoundVolume(); // Actualize the audio volume
    }

    private void LaunchGame(bool start)
    {
        gameOn = true;

        main.PlayerManager.StartPlayer();
        if (start) main.EnemiesManager.BeginChase();
        else main.EnemiesManager.ResumeEnemies();

        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            if (start) main.TeamManager.BeginProtection();
            else main.TeamManager.ResumeAttackers();
        }

        //if (main.GameAudioManager.SoundOn) main.GameAudioManager.MuteSound(false);
    }



    public void PauseGame()
    {
        main.SettingsManager.SetScreen(ScreenNumber.SETTINGS, true); // Open the setting screen

        main.PlayerManager.StopPlayer(); // Stops the player
        main.EnemiesManager.StopEnemies(); // Stops the enemies

        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
            main.TeamManager.StopAttackers(); // Stops the attackers

        //if (main.GameAudioManager.SoundOn) main.GameAudioManager.MuteSound(true);
    }
    public void UnpauseGame()
    {
        main.SettingsManager.SetScreen(ScreenNumber.SETTINGS, false);

        StartCoroutine(UnpauseCR(0.5f));
    }

    private IEnumerator UnpauseCR(float time)
    {
        int i = 3;
        while (i != 0)
        {
            // Display i
            yield return new WaitForSeconds(time);
            i--;
        }

        LaunchGame(false);
    }


    private IEnumerator GameOverCR()
    {
        main.DataManager.PostScore(gameData, score);
        
        main.PlayerManager.DeadPlayer();
        main.GameUIManager.GameOver();

        // Call the Ouuuuuh with the game audio manager (currently in field manager)

        yield return new WaitForSeconds(0.75f);

        main.FieldManager.GameOver();
        main.EnemiesManager.StopEnemies();
        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            main.TeamManager.StopAttackers(); // Stop attackers
        }

        // Call the Booouuh with the game audio manager
    }
}
