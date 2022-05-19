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
