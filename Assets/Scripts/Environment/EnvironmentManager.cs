using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnvironmentStyle { SUN = 0, RAIN = 1, NIGHT = 2, ZOMBIE = 3, FOG = 4 }

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

    [Header("Rain")]
    [SerializeField] private float rainFogDensity;
    [SerializeField] private int normalRainAddition;
    [SerializeField] private int hardRainAddition;

    [Header("Fog")]
    [SerializeField] private float normalFogIntensity;
    [SerializeField] private float objectifFogIntensity;

    [SerializeField] private float normalFogIncrease;
    [SerializeField] private float hardFogIncrease;

    [SerializeField] private Color baseFogColor;
    [SerializeField] private Color zombieFogColor;



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
        // # Weather #
        if (main.GameManager.gameData.gameWeather == GameWeather.RAIN) // RAIN
        {
            envStyleNumber = EnvironmentStyle.RAIN;

            main.PlayerManager.player.effects.Rain(true, 0);
            main.PlayerManager.player.controller.Rain();

            RenderSettings.fog = true;
            RenderSettings.fogDensity = rainFogDensity;
            RenderSettings.fogColor = main.GameManager.gameData.stadium.GetComponentInChildren<Stadium>().fogColor;
        }
        if (main.GameManager.gameData.gameWeather == GameWeather.FOG) // FOG
        {
            envStyleNumber = EnvironmentStyle.FOG;
            RenderSettings.fog = true;
            RenderSettings.fogColor = main.GameManager.gameData.stadium.GetComponentInChildren<Stadium>().fogColor;

            // # Options #
            if (main.GameManager.gameData.gameOptions.Contains(GameOption.OBJECTIF)) // OBJECTIF
                RenderSettings.fogDensity = objectifFogIntensity;
            else
                RenderSettings.fogDensity = normalFogIntensity;
        }
        if (main.GameManager.gameData.gameWeather == GameWeather.NIGHT) // NIGHT
        {
            envStyleNumber = EnvironmentStyle.NIGHT;
            RenderSettings.ambientIntensity = nightLightIntensity;
        }

        // # Modes #
        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE) // ZOMBIE
        {
            envStyleNumber = EnvironmentStyle.ZOMBIE;
            RenderSettings.ambientIntensity = zombieLightIntensity;
            RenderSettings.fogColor = zombieFogColor;
        }

        // Generates the environment
        ActuEnvironment();
    }


    public void GenerateEnvironment()
    {
        // # Modes #
        if (main.GameManager.gameData.gameMode != GameMode.ZOMBIE && main.GameManager.WaveNumber == 10) // !ZOMBIE
            BedTime(); // Night time when player reaching wave 10

        // # Weather #
        if (main.GameManager.gameData.gameWeather == GameWeather.RAIN) // RAIN
        {
            // Increases the rain according to the difficulty
            if (main.GameManager.gameData.gameDifficulty == GameDifficulty.HARD) // HARD
                IncreaseRain(hardRainAddition);
            else if (main.GameManager.gameData.gameDifficulty == GameDifficulty.NORMAL) // NORMAL
                IncreaseRain(normalRainAddition);
        }
        if (main.GameManager.gameData.gameWeather == GameWeather.FOG) // FOG
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
    private void IncreaseFog(float densityAddition)
    {
        if (main.GameManager.gameData.gameOptions.Contains(GameOption.OBJECTIF))
            densityAddition /= 2;
        RenderSettings.fogDensity += densityAddition;
    }

    private void IncreaseRain(int particleAddition)
    {
        main.PlayerManager.player.effects.Rain(true, particleAddition);
    }

    public void BedTime()
    {
        envStyleNumber = EnvironmentStyle.NIGHT;
        Destroy(dirLight.gameObject);
        ActuEnvironment();
        RenderSettings.ambientIntensity = nightLightIntensity;
    }
}
