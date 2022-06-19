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
    [HideInInspector] public DataManager dataManager { get; private set; }

    // Menu scene managers
    private MenuUIManager menuUIManager;

    // Game scene managers
    private MainManager mainManager;

    private GameManager gameManager;
    private PlayerManager playerManager;


    [Header("Settings screens")]
    [SerializeField] private GameObject[] screens;


    [Header("UI elements")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider smoothRotationSlider;
    [SerializeField] private Slider viewTypeSlider;

    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Toggle soundOnToggle;

    [SerializeField] private Toggle infoButtonsToggle;

    // ### Properties ###
    public float YMouseSensitivity
    {
        set 
        {
            dataManager.gameplayData.yms = value;
            if (playerManager != null) playerManager.YMouseSensitivity = value;
        }
    }
    public float YSmoothRotation
    {
        set 
        {
            dataManager.gameplayData.ysr = value;
            if (playerManager != null) playerManager.YSmoothRotation = value;
        }
    }

    public float ViewType
    {
        set 
        {
            dataManager.gameplayData.viewType = (ViewType) value;
            if (playerManager != null) playerManager.ViewType = (ViewType) value;


            if (value == 0)
            {
                sensitivitySlider.interactable = true;
                smoothRotationSlider.interactable = true;
            }
            else if (value == 1)
            {
                sensitivitySlider.interactable = false;
                smoothRotationSlider.interactable = false;
            }
        }
    }

    public bool InfoButtonsOn
    {
        get { return dataManager.playerPrefs.infoButtonsOn; }
        set 
        {
            dataManager.playerPrefs.infoButtonsOn = value;
            if (menuUIManager != null) menuUIManager.InfoButtonsOn = value;
        }
    }

    public bool SoundOn { set { dataManager.audioData.soundOn = value; } }
    public float SoundVolume { set { dataManager.audioData.soundVolume = value; } }


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
    }

    /// <summary>
    /// Gets the Data Manager Instance
    /// </summary>
    private void Start()
    {
        dataManager = DataManager.InstanceDataManager;

        GetManagers();

        LoadGameplayData(dataManager.gameplayData);
        LoadAudioData(dataManager.audioData);
        LoadPlayerPrefs(dataManager.playerPrefs);

        SetEventSystem(true);
    }



    // ### Functions ###

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
            mainManager = FindObjectOfType<MainManager>();
            if (mainManager != null)
            {
                gameManager = mainManager.GameManager;
                playerManager = mainManager.PlayerManager;
            }
            else Debug.Assert(false, "Didn't find the main manager");
        }
        else if (scene == 2)
        { }

        //ActuManagers(scene);
    }
    /// <summary>
    /// Actualize the managers of the scene
    /// </summary>
    /// <param name="scene">Scene number</param>
    private void ActuManagers(int scene)
    {
        if (scene == 0)
        {
            
        }
    }

    private void LoadGameplayData(GameplayData data)
    {
        sensitivitySlider.value = data.yms;
        smoothRotationSlider.value = data.ysr;
        viewTypeSlider.value = (float) data.viewType;

        if (data.viewType == 0)
        {
            sensitivitySlider.interactable = true;
            smoothRotationSlider.interactable = true;
        }
        else if ((int) data.viewType == 1)
        {
            sensitivitySlider.interactable = false;
            smoothRotationSlider.interactable = false;
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
        if (SceneManager.GetActiveScene().buildIndex == (int) SceneNumber.GAME && !gameManager.GameOver)
        {
            gameManager.PauseGame();
        }
    }
    public void UnpauseGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneNumber.GAME && !gameManager.GameOver)
        {
            gameManager.UnpauseGame();
        }
    }

    // ## Tuto Scene


    // ### Tools

    public void SetEventSystem(bool state) { EventSystem.SetActive(state); }

    public void SetScreen(ScreenNumber screen, bool state) { screens[(int)screen].SetActive(state); }
}
