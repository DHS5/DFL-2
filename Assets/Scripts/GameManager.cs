using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


/// <summary>
/// Game Mode
/// </summary>
[System.Serializable] public enum GameMode { DEFENDERS = 0, TEAM = 1, ZOMBIE = 2, DRILL = 3, TUTORIAL = 4 }

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
/// Game Option
/// </summary>
[System.Serializable] public enum ViewType { FPS = 0, TPS = 1 }


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

    [Tooltip("Index of the player's prefab")]
    [Range(0, 10)] public int playerIndex;

    [Tooltip("Index of the stadium's prefab")]
    [Range(0, 10)] public int stadiumIndex;

    [Tooltip("One v One enemy's prefab")]
    public GameObject enemyPrefab;



    [Tooltip("Current wave number")]
    [HideInInspector] private int waveNumber;

    [Tooltip("Current score")]
    [HideInInspector] private int score;



    [Tooltip("Whether the game is running")]
    [HideInInspector] private bool gameOn = false;

    [Tooltip("Whether the game is over")]
    [HideInInspector] private bool gameOver = false;


    // ### Properties ###

    public int WaveNumber
    {
        get { return waveNumber; }
        set
        {
            waveNumber = value;
            main.GameUIManager.ActuWaveNumber(value);
        }
    }

    public int Score
    {
        get { return score; }
        set
        {
            if (!gameOver)
            {
                score = value;
                main.GameUIManager.ActuScore(value);
            }
        }
    }

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

                StartCoroutine(GameOverCR());
            }
        }
    }


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    /// <summary>
    /// Starts the game
    /// </summary>
    private void Start()
    {
        GetGameDatas();

        PrepareGame(true);

        LaunchGame(false);
    }


    /// <summary>
    /// Checks if the game is on and whether the game is over
    /// </summary>
    private void Update()
    {
        // Pause the game on press P
        if (Input.GetKeyDown(KeyCode.P) && GameOn) GameOn = false;

        if (main.PlayerManager.player.gameplay.onField)
        {
            Score = CalculateScore();
        }
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

            gameData.playerIndex = playerIndex;
            gameData.stadiumIndex = stadiumIndex;
            gameData.enemy = enemyPrefab;
        }
        gameData.gameEnemiesRange = enemiesRange;

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

        main.EnemiesManager.SuppEnemies(); // Destroys all the enemies
        main.FieldManager.DestroyField(); // Destroys the former field
    }

    /// <summary>
    /// Generates the new field and everything with it
    /// </summary>
    /// <param name="start">If true generates the first field of the game, if false generates a new field</param>
    private void PrepareGame(bool start)
    {
        // # Essential #
        if (start)
        {
            main.PlayerManager.PreparePlayer();
            main.EnvironmentManager.StartEnvironment(); // Environment
        }
        else main.EnvironmentManager.GenerateEnvironment();
        main.FieldManager.GenerateField(); // Field
        main.EnemiesManager.EnemyWave(); // Enemies


        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
            main.TeamManager.TeamCreation();


        // # Options #
        if (gameData.gameOptions.Contains(GameOption.BONUS))
            main.BonusManager.GenerateBonus();

        if (gameData.gameOptions.Contains(GameOption.OBSTACLE))
            main.ObstacleManager.GenerateObstacles((waveNumber + (int) gameData.gameDifficulty) * 5);

        if (gameData.gameOptions.Contains(GameOption.OBJECTIF))
            main.ObjectifManager.GenerateObj();

        if (gameData.gameOptions.Contains(GameOption.WEAPONS)) { }
            //main.


        main.GameAudioManager.GenerateAudio(); // Actualize the audio volume etc...
    }

    /// <summary>
    /// Launches the game by activating the player, the enemies, the atackers...
    /// </summary>
    /// <param name="pause">If true resumes the game after a pause, if false launches a new wave</param>
    private void LaunchGame(bool pause)
    {
        gameOn = true;

        main.PlayerManager.StartPlayer();
        if (!pause) main.EnemiesManager.BeginChase();
        else main.EnemiesManager.ResumeEnemies();

        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            if (!pause) main.TeamManager.BeginProtection();
            else main.TeamManager.ResumeAttackers();
        }

        // # Audio #
        main.GameAudioManager.GenerateAudio();
    }

    public void EnterField()
    {
        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            main.TeamManager.BeginProtection();
        }
    }


    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        gameOn = false;

        Time.timeScale = 0f;

        main.CursorManager.UnlockCursor();
        
        main.SettingsManager.SetScreen(ScreenNumber.SETTINGS, true); // Open the setting screen


        // # Data #
        main.DataManager.SavePlayerData();

        // # Audio #
        if (main.DataManager.audioData.soundOn) main.GameAudioManager.MuteSound(true);
    }

    /// <summary>
    /// Unpause the game
    /// Close the settings screen and start UnpauseCR coroutine
    /// </summary>
    public void UnpauseGame()
    {
        main.CursorManager.LockCursor();
        
        main.SettingsManager.SetScreen(ScreenNumber.SETTINGS, false);

        StartCoroutine(UnpauseCR(0.5f));


        // # Data #
        main.DataManager.SavePlayerData();
    }

    /// <summary>
    /// Displays a 3 2 1 timer before launching the game
    /// </summary>
    /// <param name="time">Time between display (in s)</param>
    /// <returns></returns>
    private IEnumerator UnpauseCR(float time)
    {
        main.SettingsManager.SetEventSystem(false);
        
        int i = 3;
        while (i > 0)
        {
            main.GameUIManager.ResumeGameText(i, true);
            yield return new WaitForSecondsRealtime(time);
            i--;
        }

        Time.timeScale = 1.0f;

        main.GameUIManager.ResumeGameText(3, false);

        main.SettingsManager.SetEventSystem(true);

        gameOn = true;
    }
    /// <summary>
    /// Executes game over tasks with a certain timing
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverCR()
    {
        // # Data #
        main.DataManager.SavePlayerData();


        //main.PlayerManager.DeadPlayer();
        main.PlayerManager.StopPlayer();
        main.GameUIManager.GameOver();

        // Call the Ouuuuuh with the game audio manager (currently in field manager)

        yield return new WaitForSeconds(0.75f);

        main.FieldManager.GameOver();
        main.EnemiesManager.GameOver();
        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            main.TeamManager.StopAttackers(); // Stop attackers
        }

        // Call the Booouuh with the game audio manager
    }


    /// <summary>
    /// Called when a wave is passed by the player
    /// Creates and launches the next wave
    /// </summary>
    public void NextWave()
    {
        WaveNumber++;

        CleanGame();

        PrepareGame(false);

        LaunchGame(false);
    }


    private int CalculateScore()
    {
        return waveNumber * 100 -
                (int)(main.FieldManager.field.fieldZone.transform.position.z +
                main.FieldManager.field.fieldZone.transform.localScale.z / 2 -
                main.PlayerManager.player.transform.position.z) / 
                (int) (main.FieldManager.field.fieldZone.transform.localScale.z / 100);
    }
}
