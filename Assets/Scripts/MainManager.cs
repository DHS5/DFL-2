using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager InstanceMainManager { get; private set; }


    // Multi scene managers
    [HideInInspector] public DataManager DataManager { get; private set; }
    [HideInInspector] public SettingsManager SettingsManager { get; private set; }

    // Game scene managers
    // Essentials
    public GameManager GameManager { get { return gameManager; } }
    public GameManager gameManager;
    public PlayerManager PlayerManager { get { return playerManager; } }
    public PlayerManager playerManager;
    public CursorManager CursorManager { get { return cursorManager; } }
    public CursorManager cursorManager;
    public FieldManager FieldManager { get { return fieldManager; } }
    public FieldManager fieldManager;
    public EnvironmentManager EnvironmentManager { get { return environmentManager; } }
    public EnvironmentManager environmentManager;
    public GameUIManager GameUIManager { get { return gameUIManager; } }
    public GameUIManager gameUIManager;
    public GameAudioManager GameAudioManager { get { return gameAudioManager; } }
    public GameAudioManager gameAudioManager;
    public EnemiesManager EnemiesManager { get { return enemiesManager; } }
    public EnemiesManager enemiesManager;
    public MissionManager MissionManager { get { return missionManager; } }
    public MissionManager missionManager;

    // Modes
    public TeamManager TeamManager { get { return teamManager; } }
    public TeamManager teamManager;
    public TutorialManager TutoManager { get { return tutoManager; } }
    public TutorialManager tutoManager;

    // Options
    public ObjectifManager ObjectifManager { get { return objectifManager; } }
    public ObjectifManager objectifManager;
    public ObstacleManager ObstacleManager { get { return obstacleManager; } }
    public ObstacleManager obstacleManager;
    public BonusManager BonusManager { get { return bonusManager; } }
    public BonusManager bonusManager;
    public WeaponsManager WeaponsManager { get { return weaponsManager; } }
    public WeaponsManager weaponsManager;

    // Drills
    public ParkourManager ParkourManager { get { return parkourManager; } }
    public ParkourManager parkourManager;


    private void Awake()
    {
        InstanceMainManager = this;
        
        // # Multi scene managers
        DataManager = DataManager.InstanceDataManager;
        SettingsManager = SettingsManager.Instance;

        // # Game scene managers

        // Modes
        if (DataManager.gameData.gameMode != GameMode.TEAM) TeamManager.enabled = false;
        if (DataManager.gameData.gameMode != GameMode.TUTORIAL) TutoManager.enabled = false;

        // Options
        if (!DataManager.gameData.gameOptions.Contains(GameOption.OBJECTIF) && !(DataManager.gameData.gameDrill == GameDrill.OBJECTIF)) ObjectifManager.enabled = false;
        if (!DataManager.gameData.gameOptions.Contains(GameOption.OBSTACLE)) ObstacleManager.enabled = false;
        if (!DataManager.gameData.gameOptions.Contains(GameOption.BONUS)) BonusManager.enabled = false;
        if (!DataManager.gameData.gameOptions.Contains(GameOption.WEAPONS)) WeaponsManager.enabled = false;

        // Drills
        if (DataManager.gameData.gameMode != GameMode.DRILL || DataManager.gameData.gameDrill != GameDrill.PARKOUR) ParkourManager.enabled = false;
    }
}
