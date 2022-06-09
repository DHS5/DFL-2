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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
