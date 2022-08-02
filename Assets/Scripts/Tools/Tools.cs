using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public void LoadScene(int scene)
    {
        SaveData();

        if (SceneManager.GetActiveScene().buildIndex != scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
    public void LoadScene(SceneNumber scene)
    {
        LoadScene((int)scene);
    }

    public void ReloadScene()
    {
        SaveData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitInstant()
    {
        SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif UNITY_WEBGL
#else
        Application.Quit();
#endif
    }


    private void SaveData()
    {
        DataManager dataManager = DataManager.InstanceDataManager;
        if (dataManager != null)
            dataManager.SaveDatas();
    }


    // # UI #

    public void InverseState(GameObject g)
    {
        g.SetActive(!g.activeSelf);
    }
}
