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
[System.Serializable] public enum GameDifficulty { NULL = -1, ROOKIE = 0 , PRO = 1 , STAR = 2 , VETERAN = 3 , LEGEND = 4 }

/// <summary>
/// Game Option
/// </summary>
[System.Serializable] public enum GameOption { BONUS = 0, OBSTACLE = 1, OBJECTIF = 2, WEAPONS = 3 }

/// <summary>
/// Game Option
/// </summary>
[System.Serializable] public enum GameWeather { NULL = -1, SUN = 0, RAIN = 1, FOG = 2, NIGHT = 3 }

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
    [Tooltip("Main Manager")]
    private MainManager main;


    [HideInInspector] public GameData gameData;


    [Header("Game parameters")]

    [Tooltip("Range of different enemies that can spawn in one wave")]
    [Range(0, 5)] public int enemiesRange;




    [Tooltip("Current wave number")]
    private int waveNumber;

    [Tooltip("Current score")]
    private int score;



    [Tooltip("Whether the game is running")]
    private bool gameOn = false;

    [Tooltip("Whether the game is over")]
    private bool gameOver = false;


    private bool transitionning = false;
    private float prevTimescale = 1.0f;

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
            if (transitionning) return;
            if (GameOver)
            {
                main.SettingsManager.SetScreen(ScreenNumber.SETTINGS, !value);
                return;
            }
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

        LaunchGame();

        Physics.IgnoreLayerCollision(7, 8);
        Physics.IgnoreLayerCollision(8, 9);
    }


    /// <summary>
    /// Checks if the game is on and whether the game is over
    /// </summary>
    private void Update()
    {
        // Pause the game on press P
        if (Input.GetKeyDown(KeyCode.Tab)) GameOn = !GameOn;

        if (GameOver && Input.GetKeyDown(KeyCode.Return)) Tools.ReloadScene();

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

        gameData.gameEnemiesRange = enemiesRange;

        WaveNumber = 1;
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
            main.GameUIManager.SetBackview(main.DataManager.gameplayData.backview);
        }
        else main.EnvironmentManager.GenerateEnvironment();
        main.FieldManager.GenerateField(); // Field
        main.EnemiesManager.EnemyWave(); // Enemies
        if (start) main.PlayerManager.PositionPlayer();


        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
            main.TeamManager.TeamCreation();
        if (gameData.gameMode == GameMode.DRILL)
        {
            gameData.gameOptions.Clear();
            if (gameData.gameDrill == GameDrill.OBJECTIF)
                main.ObjectifManager.GenerateObj(3 + ((int) gameData.gameDifficulty + waveNumber) * 2);
        }


        // # Options #
        if (gameData.gameOptions.Contains(GameOption.BONUS))
            main.BonusManager.GenerateBonus();

        if (gameData.gameOptions.Contains(GameOption.OBSTACLE))
            main.ObstacleManager.GenerateObstacles(10 + (waveNumber + (int) gameData.gameDifficulty) * 5);

        if (gameData.gameOptions.Contains(GameOption.OBJECTIF))
            main.ObjectifManager.GenerateObj();

        if (gameData.gameOptions.Contains(GameOption.WEAPONS))
            main.WeaponsManager.GenerateWeaponBonus();

        // # Drills #
        if (gameData.gameMode == GameMode.DRILL && gameData.gameDrill == GameDrill.PARKOUR)
            main.ParkourManager.GenerateParkour();


        main.GameAudioManager.GenerateAudio(); // Actualize the audio volume etc...
    }

    /// <summary>
    /// Launches the game by activating the player, the enemies, the atackers...
    /// </summary>
    private void LaunchGame()
    {
        gameOn = true;

        Time.timeScale = 1.0f;

        main.PlayerManager.StartPlayer();
        main.EnemiesManager.BeginChase();

        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            main.TeamManager.BeginProtection();
        }
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

        prevTimescale = Time.timeScale;
        Time.timeScale = 0f;

        main.CursorManager.UnlockCursor();
        
        main.SettingsManager.SetScreen(ScreenNumber.SETTINGS, true); // Open the setting screen


        // # Audio #
        main.GameAudioManager.Pause(true);
    }

    /// <summary>
    /// Unpause the game
    /// Close the settings screen and start UnpauseCR coroutine
    /// </summary>
    public void UnpauseGame()
    {
        transitionning = true;

        main.CursorManager.LockCursor();
        
        main.SettingsManager.SetScreen(ScreenNumber.ALL, false);

        // # Audio #
        main.GameAudioManager.Pause(false);

        StartCoroutine(UnpauseCR(0.5f));
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

        Time.timeScale = prevTimescale;

        main.GameUIManager.ResumeGameText(3, false);

        main.SettingsManager.SetEventSystem(true);

        gameOn = true;

        transitionning = false;
    }

    /// <summary>
    /// Executes game over tasks with a certain timing
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverCR()
    {
        if (gameData.gameMode != GameMode.DRILL && gameData.gameMode != GameMode.TUTORIAL)
        {
            // # Leaderboard #
            LeaderboardManager.PostScore(gameData, Score, WaveNumber);

            // # Stats #
            StatsManager.AddGameToStats(gameData, Score, WaveNumber);
        }

        // # Coins #
        int coins = CoinsManager.GameCoins(gameData, Score, WaveNumber, main.WeaponsManager.numberOfKill, main.FieldManager.stadium.coinsPercentage);
        main.DataManager.inventoryData.coins += coins;
        main.GameUIManager.ActuCoins(gameData, Score, WaveNumber, main.WeaponsManager.numberOfKill, main.FieldManager.stadium.coinsPercentage, coins);

        // # Missions #
        main.MissionManager.CompleteMissions(gameData, waveNumber);

        // # Data #
        main.DataManager.SaveDatas(false);

        // # UI #
        if (gameData.gameMode == GameMode.DRILL && gameData.gameDrill == GameDrill.PARKOUR) main.GameUIManager.Lose();
        else main.GameUIManager.GameOver();

        // Weapons
        if (gameData.gameMode == GameMode.ZOMBIE && gameData.gameOptions.Contains(GameOption.WEAPONS))
        {
            main.GameUIManager.ActuKills(main.WeaponsManager.numberOfKill);
            main.WeaponsManager.GameOver();
        }

        // # Audios #
        main.GameAudioManager.GetSoundSources();
        main.GameAudioManager.Lose();

        // Call the Ouuuuuh with the game audio manager (currently in field manager)

        yield return new WaitForSeconds(0.75f);

        main.FieldManager.GameOver();
        main.EnemiesManager.GameOver();
        // # Modes #
        if (gameData.gameMode == GameMode.TEAM)
        {
            main.TeamManager.StopAttackers(); // Stop attackers
        }

        // # Weather #
        if (gameData.gameWeather == GameWeather.RAIN)
        {
            main.FieldManager.stadium.Rain();
        }

        // Call the Booouuh with the game audio manager

        // # Audios #
        main.GameAudioManager.GetSoundSources();
    }


    public void Win()
    {
        gameOver = true;

        // # Parkour #
        main.ParkourManager.Win();

        // # Data #
        main.DataManager.SaveDatas(false);

        // # UI #
        main.GameUIManager.Win();

        // # Cursor #
        main.CursorManager.UnlockCursor();

        // # Audios #
        main.GameAudioManager.Win();
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

        LaunchGame();
    }



    public void ViewChange()
    {
        if (gameData.gameOptions.Contains(GameOption.WEAPONS))
            main.WeaponsManager.ViewChange();
    }



    private int CalculateScore()
    {
        return waveNumber * 100 -
                (int)((main.FieldManager.field.fieldZone.transform.position.z +
                main.FieldManager.field.fieldZone.transform.localScale.z / 2 -
                main.PlayerManager.player.transform.position.z) / 
                (main.FieldManager.field.fieldZone.transform.localScale.z / 100));
    }
}
