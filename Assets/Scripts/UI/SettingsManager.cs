using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum SceneNumber { MENU = 0, GAME = 1, TUTO = 2 }


public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Settings Manager of the game
    /// </summary>
    public static SettingsManager InstanceSettingsManager { get; private set; }

    // ### Managers ###
    // Multi scene managers
    [HideInInspector] public DataManager dataManager { get; private set; }

    // Menu scene managers
    private MenuUIManager menuUIManager;

    // Game scene managers
    private MainManager mainManager;

    private GameManager gameManager;
    private PlayerManager playerManager;


    [Header("UI elements")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider smoothRotationSlider;

    private bool infoButtonsOn = true;


    // ### Properties ###
    public float YMouseSensitivity
    {
        set 
        { 
            dataManager.yMouseSensitivity = value;
            if (playerManager != null) playerManager.YMouseSensitivity = value;
        }
    }
    public float YSmoothRotation
    {
        set 
        { 
            dataManager.ySmoothRotation = value;
            if (playerManager != null) playerManager.YSmoothRotation = value;
        }
    }

    public bool InfoButtonsOn
    {
        get { return infoButtonsOn; }
        set 
        {
            infoButtonsOn = value;
            if (menuUIManager != null) menuUIManager.InfoButtonsOn = value;
        }
    }


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

        ActuManagers(scene);
    }
    /// <summary>
    /// Actualize the managers of the scene
    /// </summary>
    /// <param name="scene">Scene number</param>
    private void ActuManagers(int scene)
    {
        if (scene == 0)
        {
            menuUIManager.InfoButtonsOn = infoButtonsOn;
        }
    }

    // ## Menu Scene

    // ## Game Scene

    // ## Tuto Scene
}
