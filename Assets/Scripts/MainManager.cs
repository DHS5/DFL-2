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
    [HideInInspector] public GameManager GameManager { get; private set; }
    [HideInInspector] public PlayerManager PlayerManager { get; private set; }
    [HideInInspector] public FieldManager FieldManager { get; private set; }
    [HideInInspector] public EnvironmentManager EnvironmentManager { get; private set; }
    [HideInInspector] public GameUIManager GameUIManager { get; private set; }
    [HideInInspector] public GameAudioManager GameAudioManager { get; private set; }
    [HideInInspector] public EnemiesManager EnemiesManager { get; private set; }

    // Modes
    [HideInInspector] public TeamManager TeamManager { get; private set; }
    
    // Options
    [HideInInspector] public ObjectifManager ObjectifManager { get; private set; }
    [HideInInspector] public ObstacleManager ObstacleManager { get; private set; }
    [HideInInspector] public BonusManager BonusManager { get; private set; }


    private void Awake()
    {
        InstanceMainManager = this;
        
        // # Multi scene managers
        DataManager = DataManager.InstanceDataManager;
        SettingsManager = SettingsManager.InstanceSettingsManager;

        // # Game scene managers
        // Essentials
        GameManager = GetComponent<GameManager>();
        PlayerManager = GetComponent<PlayerManager>();
        FieldManager = GetComponent<FieldManager>();
        EnvironmentManager = GetComponent<EnvironmentManager>();
        GameUIManager = GetComponent<GameUIManager>();
        GameAudioManager = GetComponent<GameAudioManager>();
        EnemiesManager = GetComponent<EnemiesManager>();

        // Modes
        TeamManager = GetComponent<TeamManager>();

        // Options
        ObjectifManager = GetComponent<ObjectifManager>();
        ObstacleManager = GetComponent<ObstacleManager>();
        BonusManager = GetComponent<BonusManager>();


        SettingsManager.GetManagers(); // Makes the settings manager get the useful managers
    }
}
