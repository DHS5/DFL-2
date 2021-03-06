using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMainManager : MonoBehaviour
{
    public static MenuMainManager Instance { get; private set; }


    // Multi scene managers
    public DataManager DataManager { get; private set; }
    public SettingsManager SettingsManager { get; private set; }
    public LoginManager LoginManager { get; private set; }

    // Menu scene managers
    public MenuUIManager MenuUIManager { get; private set; }
    public LeaderboardManager LeaderboardManager { get; private set; }
    public StatsManager StatsManager { get; private set; }
    public ShopManager ShopManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    public ProgressionManager ProgressionManager { get; private set; }


    private void Awake()
    {
        Instance = this;

        // # Multi scene managers
        DataManager = FindObjectOfType<DataManager>();
        SettingsManager = FindObjectOfType<SettingsManager>();
        LoginManager = FindObjectOfType<LoginManager>();

        // # Game scene managers
        MenuUIManager = GetComponent<MenuUIManager>();
        LeaderboardManager = GetComponent<LeaderboardManager>();
        StatsManager = GetComponent<StatsManager>();
        ShopManager = GetComponent<ShopManager>();
        InventoryManager = GetComponent<InventoryManager>();
        ProgressionManager = GetComponent<ProgressionManager>();

        SettingsManager.GetManagers(); // Makes the settings manager get the useful managers
    }

    private void Start()
    {
        DataManager.ApplyResults();
    }
}
