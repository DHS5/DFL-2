using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMainManager : MonoBehaviour
{
    public static MenuMainManager Instance { get; private set; }


    // Multi scene managers
    public DataManager DataManager { get; private set; }
    public SettingsManager SettingsManager { get; private set; }
    public MusicSource MusicSource { get; private set; }

    // Menu scene managers
    public MenuUIManager MenuUIManager { get; private set; }
    public LeaderboardManager LeaderboardManager { get; private set; }
    public StatsManager StatsManager { get; private set; }
    public ShopManager ShopManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    public ProgressionManager ProgressionManager { get; private set; }
    public LoginManager LoginManager { get; private set; }
    public CardManager CardManager { get; private set; }


    private void Awake()
    {
        Instance = this;

        // # Multi scene managers
        DataManager = FindObjectOfType<DataManager>();
        SettingsManager = FindObjectOfType<SettingsManager>();
        MusicSource = FindObjectOfType<MusicSource>();

        // # Game scene managers
        MenuUIManager = GetComponent<MenuUIManager>();
        LeaderboardManager = GetComponent<LeaderboardManager>();
        StatsManager = GetComponent<StatsManager>();
        ShopManager = GetComponent<ShopManager>();
        InventoryManager = GetComponent<InventoryManager>();
        ProgressionManager = GetComponent<ProgressionManager>();
        LoginManager = GetComponent<LoginManager>();
        CardManager = GetComponent<CardManager>();

        SettingsManager.GetManagers(); // Makes the settings manager get the useful managers
    }
}
