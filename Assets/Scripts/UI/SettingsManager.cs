using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// DataManager of the game
    /// </summary>
    public static SettingsManager InstanceSettingsManager { get; private set; }


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

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
