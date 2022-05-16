using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public enum SceneNumber { MENU = 0, GAME = 1, TUTO = 2 }


public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Settings Manager of the game
    /// </summary>
    public static SettingsManager InstanceSettingsManager { get; private set; }

    // Managers
    private DataManager dataManager;

    private MainManager mainManager;

    private GameManager gameManager;
    private PlayerManager playerManager;


    [Header("UI elements")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider smoothRotationSlider;


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


    /// <summary>
    /// Gets the Singleton Instance
    /// </summary>
    private void Awake()
    {
        if (InstanceSettingsManager != null)
        {
            Destroy(this);
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
    }



    // ### Functions ###

    /// <summary>
    /// Gets the managers of the current scene
    /// </summary>
    /// <param name="scene">Scene number</param>
    public void GetManagers(SceneNumber scene)
    {
        if ((int) scene == 0)
        { }
        else if ((int) scene == 1)
        {
            mainManager = FindObjectOfType<MainManager>();
            if (mainManager != null)
            {
                gameManager = mainManager.GameManager;
                playerManager = mainManager.PlayerManager;
            }
            else Debug.Assert(false, "Didn't find the main manager");
        }
        else if ((int) scene == 2)
        { }
    }

    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="scene">Scene number</param>
    public void LoadScene(int scene)
    {
        if (SceneManager.GetActiveScene().buildIndex != scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
    public void LoadScene(SceneNumber scene)
    {
        LoadScene((int)scene);
    }


    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
#else
        Application.Quit();
#endif
    }
}
