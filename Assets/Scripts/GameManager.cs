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


    private GameData gameData;

    [Header("Game parameters")]
    [Tooltip("Current game mode")]
    public GameMode gameMode;
    [Tooltip("Current game difficulty")]
    public GameDifficulty gameDifficulty;
    public GameWheather gameWheather;
    [Tooltip("Current game options")]
    public List<GameOption> gameOptions = new List<GameOption>();
    public GameDrill gameDrill;
    [Tooltip("Range of different enemies that can spawn in one wave")]
    [Range(0, 5)] public int enemiesRange;


    [Tooltip("Current field of the game")]
    [HideInInspector] public Field currentField;

    [Header("Player")]
    [Tooltip("Player Game Object")]
    public GameObject player;
    [Tooltip("Animator of the player")]
    [SerializeField] private Animator playerRunAnimator;


    [Tooltip("Whether the game is running")]
    [HideInInspector] public bool gameOn = false;
    [Tooltip("Whether the game is over")]
    [HideInInspector] public bool gameOver = false;
    private bool gameOverLate = false;


    /// <summary>
    /// Starts the game
    /// </summary>
    private void Start()
    {
        main = MainManager.InstanceMainManager;

        GetGameDatas();

        PrepareGame();

        // Game is on
        gameOn = true;
        
        // Generates the game
        TunnelEnter();

        // Unfreezes the player
        player.GetComponent<PlayerController>().freeze = false;
        // Makes the player chasable
        player.GetComponent<PlayerGameplay>().isChasable = true;

    }


    /// <summary>
    /// Checks if the game is on and whether the game is over
    /// </summary>
    private void Update()
    {
        // Checks if the game is over
        if (gameOver && !gameOverLate)
        {
            gameOverLate = true;
            playerRunAnimator.SetTrigger("Dead");
            player.GetComponent<PlayerController>().freeze = true;
            main.GameUIManager.GameOver();
            Invoke(nameof(GameOver), 0.75f);
            if (gameMode != GameMode.ZOMBIE) currentField.OuuhAudio();
            currentField.StopAmbianceAudios();
        }

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
        }
    }

    private void PrepareGame()
    {
        main.EnvironmentManager.GenerateEnvironment();
        main.FieldManager.GenerateField();
        main.EnemiesManager.EnemyWave();

        main.GameAudioManager.ActuSoundVolume();

        // Modes
        if (gameData.gameMode == GameMode.TEAM)
            main.TeamManager.TeamCreation();


        // Options
        if (gameData.gameOptions.Contains(GameOption.BONUS))
            main.BonusManager.GenerateBonus();

        if (gameData.gameOptions.Contains(GameOption.OBSTACLE))
            main.ObstacleManager.GenerateObstacles(1); // A remplacer

        if (gameData.gameOptions.Contains(GameOption.OBJECTIF))
            main.ObjectifManager.GenerateObj();
    }

    public void PauseGame()
    {
        gameOn = false;

        // Freezes the player
        player.GetComponent<PlayerController>().freeze = true;
        // Stops the enemies
        main.EnemiesManager.StopEnemies();

        // If game mode = TEAM
        if (gameMode == GameMode.TEAM)
        {
            // Stops the attackers
            main.TeamManager.StopAttackers();
        }

        main.GameUIManager.SetScreen(GameScreen.SETTINGS, true);

        //if (main.GameAudioManager.SoundOn) main.GameAudioManager.MuteSound(true);
    }
    public void UnpauseGame()
    {
        main.GameUIManager.SetScreen(GameScreen.SETTINGS, false);
        Invoke(nameof(GameOn) , 1f);
    }
    private void GameOn()
    { 
        gameOn = true;

        // Unfreezes the player
        player.GetComponent<PlayerController>().freeze = false;
        // Resumes the enemies
        main.EnemiesManager.ResumeEnemies();

        // If game mode = TEAM
        if (gameMode == GameMode.TEAM)
        {
            // Resumes the attackers
            main.TeamManager.ResumeAttackers();
        }

        player.GetComponentInChildren<FirstPersonCameraController>().LockCursor();

        //if (main.GameAudioManager.SoundOn) main.GameAudioManager.MuteSound(false);
    }


    /// <summary>
    /// Called when the player loses
    /// </summary>
    private void GameOver()
    {
        // ### Player

        // Player freezes
        //player.GetComponent<PlayerController>().freeze = true;
        // Player animator stops
        //playerRunAnimator.SetTrigger("Dead");


        // ### Enemies

        // Enemies stop
        main.EnemiesManager.StopEnemies();

        
        // ### Modes

        // If game mode = TEAM
        if (gameMode == GameMode.TEAM)
        {
            // Attackers stop
            main.TeamManager.StopAttackers();
        }


        // ### Environment

        // Activate the stadium's camera
        main.FieldManager.StadiumCamera.gameObject.SetActive(true);


        // ### UI

        // Calls the UI restart screen
        //gameUIManager.GameOver();


        // ### Audio

        // Calls the booh audios
        if (gameMode != GameMode.ZOMBIE) currentField.BoohAudio();


        // ### Data

        // Save the new score
        //if (dataManager.NewScore(gameMode, difficulty, options, "DHS5", enemiesManager.waveNumber) == 1)
        //{
        //    dataManager.SaveHighscores();
        //}
        if (main.DataManager != null && main.EnemiesManager.waveNumber > 1)
        {
            //int index = dataManager.IsNewHighscoreF(gameMode, difficulty, options, enemiesManager.waveNumber);
            if (main.DataManager.IsNewHighscoreO(gameMode, gameDifficulty, gameOptions, main.EnemiesManager.waveNumber))
            {
                //main.DataManager.highWave = main.EnemiesManager.waveNumber;
                //dataManager.highIndex = index;
                main.GameUIManager.SetScreen(GameScreen.ONLINE_HIGHSCORE, true);
                main.GameUIManager.SetScreen(GameScreen.RESTART, false);
                main.GameUIManager.ActuInputField();
            }
            //else if (index != -1)
            //{
            //    dataManager.highWave = enemiesManager.waveNumber;
            //    dataManager.highIndex = index;
            //    gameUIManager.SetScreen(GameScreen.HIGHSCORE, true);
            //    gameUIManager.SetScreen(GameScreen.RESTART, false);
            //    gameUIManager.ActuInputField();
            //}
        }
    }


    /// <summary>
    /// Called when the player enters the tunnel
    /// </summary>
    public void TunnelEnter()
    {
        // ### Environment
        
        // Generates a new field
        currentField = main.FieldManager.GenerateField();
        // Increases the fog according to the difficulty
        if (gameDifficulty == GameDifficulty.HARD) main.EnvironmentManager.IncreaseFog(0.03f);
        else if (gameDifficulty == GameDifficulty.NORMAL) main.EnvironmentManager.IncreaseFog(0.01f);
        // Passes to night mode after wave 10 (if not zombie mode)
        if (gameMode != GameMode.ZOMBIE && main.EnemiesManager.waveNumber == 9) main.EnvironmentManager.BedTime();


        // ### Audio

        // Actualize the audio volume
        
        // Disable the crowd sound in zombie mode
        if (gameMode == GameMode.ZOMBIE) currentField.StopAmbianceAudios();

        // ### Enemies

        // Generates an enemy wave


        // ### Modes

        // If game mode = TEAM
        if (gameMode == GameMode.TEAM)
        {
            // Clear the attackers
            main.TeamManager.ClearAttackers();
            // Gives the teamManager an enemies's clone
            main.TeamManager.enemies = new List<GameObject>(currentField.enemies);
        }


        // ### Options

        // If the option OBSTACLE is chosen
        if (gameOptions.Contains(GameOption.OBSTACLE))
        {
            // Destroys the active obstacles
            main.ObstacleManager.DestroyObstacles();
            // Generates the obstacles
            main.ObstacleManager.GenerateObstacles((main.EnemiesManager.waveNumber + (int) gameDifficulty) * 5);
        }

        // If the option BONUS is chosen
        if (gameOptions.Contains(GameOption.BONUS))
        {
            // Destroys the active bonus
            main.BonusManager.DestroyBonus();
            // Generates the bonus
            main.BonusManager.GenerateBonus();
        }
    }

    /// <summary>
    /// Called when the player exits the tunnel
    /// </summary>
    public void TunnelExit()
    {
        // ### Environment

        // Destroys the former field
        main.FieldManager.DestroyField();


        // ### Enemies

        // Starts the enemies's chase
        main.EnemiesManager.BeginChase();


        // ### Modes

        // If game mode = TEAM
        if (gameMode == GameMode.TEAM)
        {
            // Creates a team and starts the player's protection
            main.TeamManager.TeamCreation();
            main.TeamManager.BeginProtection();
        }

        // If game mode = OBJECTIF
        //if (gameMode == GameMode.OBJECTIF)
        //{
        //    // Generates the objectives
        //    objectifManager.GenerateObj();
        //}

        // ### Audio

        // Actualize the audio volume
        main.GameAudioManager.ActuSoundVolume();
    }
}
