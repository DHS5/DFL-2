using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Multi scene managers
    private DataManager dataManager;
    [HideInInspector] public SettingsManager SettingsManager { get; private set; }

    // Game scene managers
    [HideInInspector] public GameManager GameManager { get; private set; }
    [HideInInspector] public PlayerManager PlayerManager { get; private set; }



    private void Awake()
    {
        // Multi scene managers
        dataManager = DataManager.InstanceDataManager;
        SettingsManager = SettingsManager.InstanceSettingsManager;

        // Game scene managers
        GameManager = GetComponent<GameManager>();
        PlayerManager = GetComponent<PlayerManager>();


        SettingsManager.GetManagers(); // Makes the settings manager get the useful managers
    }
}
