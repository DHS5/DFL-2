using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Tools : MonoBehaviour
{
    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="scene">Scene number</param>
    public static void LoadScene(int scene)
    {
        AudioListener.pause = false;
        SaveData();

        if (SceneManager.GetActiveScene().buildIndex != scene)
        {
            SceneManager.LoadScene(scene);
            Time.timeScale = 1.0f;
        }
    }
    public static void LoadScene(SceneNumber scene)
    {
        LoadScene((int)scene);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public static void QuitInstant()
    {
        SaveOnDisk();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
#else
        Application.Quit();
#endif
    }


    public static void SaveData()
    {
        DataManager dataManager = DataManager.InstanceDataManager;
        if (dataManager != null)
            dataManager.SaveAndCleanGarbage();
    }
    public static void SaveOnDisk()
    {
        DataManager dataManager = DataManager.InstanceDataManager;
        if (dataManager != null)
            dataManager.SaveOnDisk();
    }


    // # UI #

    public static void InverseState(GameObject g)
    {
        g.SetActive(!g.activeSelf);
    }

    public static void RebuildLayout(RectTransform rect)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
}
