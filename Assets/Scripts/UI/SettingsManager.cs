using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum SceneNumber { MENU = 0, GAME = 1}

public enum ScreenNumber { SETTINGS = 0, GAMEPLAY, INFO, PLAYER, LEADERBOARD, SHOP }


public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Settings Manager of the game
    /// </summary>
    public static SettingsManager InstanceSettingsManager { get; private set; }

    [SerializeField] private GameObject EventSystem;

    // ### Managers ###
    // Multi scene managers
    public DataManager DataManager { get; private set; }
    public ShopManager ShopManager { get; private set; }
    public LoginManager LoginManager { get; private set; }
    public LeaderboardManager LeaderboardManager { get; private set; }
    public StatsManager StatsManager { get; private set; }

    // Menu scene managers
    private MenuUIManager menuUIManager;

    // Game scene managers
    private MainManager main;


    [Header("Settings screens")]
    [SerializeField] private GameObject[] screens;


    [Header("UI elements")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider smoothRotationSlider;
    [SerializeField] private Slider headAngleSlider;
    [SerializeField] private Slider viewTypeSlider;
    [SerializeField] private Toggle goalpostToggle;

    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Toggle soundOnToggle;

    [SerializeField] private Toggle infoButtonsToggle;

    // ### Properties ###
    public float HeadAngle
    {
        set 
        {
            DataManager.gameplayData.headAngle = value;
            if (main.PlayerManager != null) main.PlayerManager.HeadAngle = value;
        }
    }
    public float YMouseSensitivity
    {
        set 
        {
            DataManager.gameplayData.yms = value;
            if (main.PlayerManager != null) main.PlayerManager.YMouseSensitivity = value;
        }
    }
    public float YSmoothRotation
    {
        set 
        {
            DataManager.gameplayData.ysr = value;
            if (main.PlayerManager != null) main.PlayerManager.YSmoothRotation = value;
        }
    }

    public float ViewType
    {
        set 
        {
            DataManager.gameplayData.viewType = (ViewType) value;
            if (main.PlayerManager != null) main.PlayerManager.ViewType = (ViewType) value;


            if (value == 0)
            {
                sensitivitySlider.interactable = true;
                smoothRotationSlider.interactable = true;
                headAngleSlider.interactable = true;
            }
            else if (value == 1)
            {
                sensitivitySlider.interactable = false;
                smoothRotationSlider.interactable = false;
                headAngleSlider.interactable = false;
            }
        }
    }

    public bool Goalpost
    {
        set { DataManager.gameplayData.goalpost = value; }
    }

    public bool InfoButtonsOn
    {
        get { return DataManager.playerPrefs.infoButtonsOn; }
        set 
        {
            DataManager.playerPrefs.infoButtonsOn = value;
            if (menuUIManager != null) menuUIManager.InfoButtonsOn = value;
        }
    }

    public bool SoundOn { set { DataManager.audioData.soundOn = value; } }
    public float SoundVolume { set { DataManager.audioData.soundVolume = value; } }


    /// <summary>
    /// Gets the Singleton Instance
    /// </summary>
    private void Awake()
    {
        if (InstanceSettingsManager != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceSettingsManager = this;
        DontDestroyOnLoad(gameObject);

        ShopManager = GetComponent<ShopManager>();
        LoginManager = GetComponent<LoginManager>();
        LeaderboardManager = GetComponent<LeaderboardManager>();
        StatsManager = GetComponent<StatsManager>();
    }

    /// <summary>
    /// Gets the Data Manager Instance
    /// </summary>
    private void Start()
    {
        DataManager = DataManager.InstanceDataManager;

        GetManagers();

        Load();

        SetEventSystem(true);
    }



    // ### Functions ###

    /// <summary>
    /// Load datas once DataManager has loaded all the datas
    /// </summary>
    public void Load()
    {
        LoadGameplayData(DataManager.gameplayData);
        LoadAudioData(DataManager.audioData);
        LoadPlayerPrefs(DataManager.playerPrefs);
    }



    /// <summary>
    /// Gets the managers of the current scene
    /// </summary>
    public void GetManagers()
    {
        int scene = SceneManager.GetActiveScene().buildIndex; // Finds the current scene number

        if (scene == 0)
        {
            menuUIManager = FindObjectOfType<MenuUIManager>();
        }
        else if (scene == 1)
        {
            main = FindObjectOfType<MainManager>();
            if (main == null)
                Debug.Assert(false, "Didn't find the main manager");
        }
    }

    private void LoadGameplayData(GameplayData data)
    {
        sensitivitySlider.value = data.yms;
        smoothRotationSlider.value = data.ysr;
        headAngleSlider.value = data.headAngle;
        viewTypeSlider.value = (float) data.viewType;
        goalpostToggle.isOn = data.goalpost;

        if (data.viewType == 0)
        {
            sensitivitySlider.interactable = true;
            smoothRotationSlider.interactable = true;
            headAngleSlider.interactable = true;
        }
        else if ((int) data.viewType == 1)
        {
            sensitivitySlider.interactable = false;
            smoothRotationSlider.interactable = false;
            headAngleSlider.interactable = false;
        }
    }

    private void LoadAudioData(AudioData data)
    {
        //SoundOn = data.soundOn;
        //SoundVolume = data.soundVolume;

        soundOnToggle.isOn = data.soundOn;
        soundVolumeSlider.value = data.soundVolume;
    }

    private void LoadPlayerPrefs(PlayerPrefs prefs)
    {
        infoButtonsToggle.isOn = prefs.infoButtonsOn;
    }



    // ## Menu Scene

    // ## Game Scene
    public void PauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.GAME && !main.GameManager.GameOver)
        {
            main.GameManager.PauseGame();
        }
    }
    public void UnpauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.GAME && !main.GameManager.GameOver)
        {
            main.GameManager.UnpauseGame();
        }
    }


    // ### Tools

    public void SetEventSystem(bool state) { EventSystem.SetActive(state); }

    public void SetScreen(ScreenNumber screen, bool state) { screens[(int)screen].SetActive(state); }
}
