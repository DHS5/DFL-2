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
    public MenuUIManager MenuUIManager { get { return menuUIManager; } }
    public MenuUIManager menuUIManager;
    public LeaderboardManager LeaderboardManager { get { return leaderboardManager; } }
    public LeaderboardManager leaderboardManager;
    public StatsManager StatsManager { get { return statsManager; } }
    public StatsManager statsManager;
    public ShopManager ShopManager { get { return shopManager; } }
    public ShopManager shopManager;
    public InventoryManager InventoryManager { get { return inventoryManager; } }
    public InventoryManager inventoryManager;
    public ProgressionManager ProgressionManager { get { return progressionManager; } }
    public ProgressionManager progressionManager;
    public LoginManager LoginManager { get { return loginManager; } }
    public LoginManager loginManager;
    public CardManager CardManager { get { return cardManager; } }
    public CardManager cardManager;
    public TutoUIManager TutoUIManager { get { return tutoUIManager; } }
    public TutoUIManager tutoUIManager;


    [HideInInspector] public bool awake = false;


    private void Awake()
    {
        Instance = this;

        // # Multi scene managers
        DataManager = FindObjectOfType<DataManager>();
        SettingsManager = FindObjectOfType<SettingsManager>();
        MusicSource = FindObjectOfType<MusicSource>();

        awake = true;
    }
}
