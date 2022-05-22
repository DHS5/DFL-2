using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnvironmentStyle { SUN = 0, RAIN = 1, NIGHT = 2, ZOMBIE = 3 }

public class EnvironmentManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    [SerializeField] private Material[] skyboxes;
    [SerializeField] private GameObject[] directionnalLights;

    private EnvironmentStyle envStyleNumber = EnvironmentStyle.SUN;


    private void Start()
    {
        main = GetComponent<MainManager>();
    }


    // ### Tools ###

    private void ActuEnvironment()
    {
        RenderSettings.skybox = skyboxes[(int)envStyleNumber];
        Instantiate(directionnalLights[(int)envStyleNumber], new Vector3(0, 0, 0), Quaternion.Euler(50, -30, 0));
    }



    // ### Functions ###

    public void StartEnvironment()
    {
        // # Modes #
        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE) // ZOMBIE
        {
            envStyleNumber = EnvironmentStyle.ZOMBIE;
            RenderSettings.ambientIntensity = 0.4f;
        }

        // # Wheather #
        if (main.GameManager.gameData.gameWheather == GameWheather.FOG) // FOG
        {
            RenderSettings.fog = true;

            // # Options #
            if (main.GameManager.gameData.gameOptions.Contains(GameOption.OBJECTIF)) // OBJECTIF
                RenderSettings.fogDensity = 0.03f;
            else
                RenderSettings.fogDensity = 0.1f;
        }

        // Generates the environment
        ActuEnvironment();
    }


    public void GenerateEnvironment()
    {
        // # Modes #
        if (main.GameManager.gameData.gameMode != GameMode.ZOMBIE && main.GameManager.waveNumber == 10) // !ZOMBIE
            BedTime(); // Night time when player reaching wave 10

        // # Wheather #
        if (main.GameManager.gameData.gameWheather == GameWheather.FOG) // FOG
        {
            // # Difficulties #
            // Increases the fog according to the difficulty
            if (main.GameManager.gameData.gameDifficulty == GameDifficulty.HARD) // HARD
                IncreaseFog(0.03f);
            else if (main.GameManager.gameData.gameDifficulty == GameDifficulty.NORMAL) // NORMAL
                IncreaseFog(0.01f);
        }
    }

    /// <summary>
    /// Increases the density of the fog
    /// </summary>
    /// <param name="densityAddition">FogDensity += densityAddition</param>
    public void IncreaseFog(float densityAddition)
    {
        //if (gameManager.gameMode == GameMode.OBJECTIF)
        //    densityAddition /= 2;
        RenderSettings.fogDensity += densityAddition;
    }

    public void BedTime()
    {
        envStyleNumber = EnvironmentStyle.NIGHT;
        ActuEnvironment();
        //FindObjectOfType<Light>().gameObject.SetActive(false); ???
        RenderSettings.ambientIntensity = 0.6f;
    }
}
