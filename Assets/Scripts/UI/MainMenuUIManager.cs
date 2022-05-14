using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUIManager : UIManager
{
    [SerializeField] private MenuAudioManager audioManager;



    /// <summary>
    /// Game Mode property
    /// </summary>
    public int Mode
    {
        set { DataManager.InstanceDataManager.gameMode = (GameMode)value; }
    }

    /// <summary>
    /// Difficulty property
    /// </summary>
    public int Difficulty
    {
        set { DataManager.InstanceDataManager.difficulty = (GameDifficulty)value; }
    }

    public float YMouseSensitivity
    {
        set { DataManager.InstanceDataManager.yMouseSensitivity = value; DataManager.InstanceDataManager.SavePlayerData(); }
    }
    
    public float YSmoothRotation
    {
        set { DataManager.InstanceDataManager.ySmoothRotation = value; DataManager.InstanceDataManager.SavePlayerData(); }
    }

    

    private void Start()
    {
        if (DataManager.InstanceDataManager.yMouseSensitivity != 0)
            sensitivitySlider.value = DataManager.InstanceDataManager.yMouseSensitivity;
        else YMouseSensitivity = sensitivitySlider.value;

        if (DataManager.InstanceDataManager.ySmoothRotation != 0)
            smoothRotationSlider.value = DataManager.InstanceDataManager.ySmoothRotation;
        else YSmoothRotation = smoothRotationSlider.value;

        gameType = new Vector3Int(0, 0, 0);
    }





    /// <summary>
    /// Removes or adds a game option
    /// </summary>
    /// <param name="b">True --> Add / False --> Remove</param>
    /// <param name="option">Game option to add/remove</param>
    public void ChooseOption(int option)
    {
        if (!DataManager.InstanceDataManager.options.Contains((GameOption)option)) { DataManager.InstanceDataManager.options.Add((GameOption)option); }
        else { DataManager.InstanceDataManager.options.Remove((GameOption)option); }
    }


    /// <summary>
    /// Load the game's scene
    /// </summary>
    public void LaunchGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
