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


    [Header("Environment parameters")]
    [Header("Light")]
    [SerializeField] private float zombieLightIntensity;
    [SerializeField] private float nightLightIntensity;

    [Header("Fog")]
    [SerializeField] private float normalFogIntensity;
    [SerializeField] private float objectifFogIntensity;

    [SerializeField] private float normalFogIncrease;
    [SerializeField] private float hardFogIncrease;



    readonly Quaternion dirLightRotation = Quaternion.Euler(50, -30, 0);

    private EnvironmentStyle envStyleNumber;

    private Light dirLight;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Tools ###

    private void ActuEnvironment()
    {
        RenderSettings.skybox = skyboxes[(int)envStyleNumber];
        dirLight = Instantiate(directionnalLights[(int)envStyleNumber], Vector3.zero, dirLightRotation).GetComponent<Light>();
    }



    // ### Functions ###

    public void StartEnvironment()
    {
        // # Modes #
        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE) // ZOMBIE
        {
            envStyleNumber = EnvironmentStyle.ZOMBIE;
            RenderSettings.ambientIntensity = zombieLightIntensity;
        }

        // # Wheather #
        if (main.GameManager.gameData.gameWheather == GameWheather.FOG) // FOG
        {
            RenderSettings.fog = true;

            // # Options #
            if (main.GameManager.gameData.gameOptions.Contains(GameOption.OBJECTIF)) // OBJECTIF
                RenderSettings.fogDensity = objectifFogIntensity;
            else
                RenderSettings.fogDensity = normalFogIntensity;
        }

        // Generates the environment
        ActuEnvironment();
    }


    public void GenerateEnvironment()
    {
        // # Modes #
        if (main.GameManager.gameData.gameMode != GameMode.ZOMBIE && main.GameManager.WaveNumber == 10) // !ZOMBIE
            BedTime(); // Night time when player reaching wave 10

        // # Wheather #
        if (main.GameManager.gameData.gameWheather == GameWheather.FOG) // FOG
        {
            // # Difficulties #
            // Increases the fog according to the difficulty
            if (main.GameManager.gameData.gameDifficulty == GameDifficulty.HARD) // HARD
                IncreaseFog(hardFogIncrease);
            else if (main.GameManager.gameData.gameDifficulty == GameDifficulty.NORMAL) // NORMAL
                IncreaseFog(normalFogIncrease);
        }
    }

    /// <summary>
    /// Increases the density of the fog
    /// </summary>
    /// <param name="densityAddition">FogDensity += densityAddition</param>
    public void IncreaseFog(float densityAddition)
    {
        if (main.GameManager.gameOptions.Contains(GameOption.OBJECTIF))
            densityAddition /= 2;
        RenderSettings.fogDensity += densityAddition;
    }

    public void BedTime()
    {
        envStyleNumber = EnvironmentStyle.NIGHT;
        Destroy(dirLight.gameObject);
        ActuEnvironment();
        RenderSettings.ambientIntensity = nightLightIntensity;
    }
}
