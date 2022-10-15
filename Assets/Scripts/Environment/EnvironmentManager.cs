using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnvironmentStyle { SUN = 0, RAIN = 1, NIGHT = 2, ZOMBIE = 3, FOG = 4, SUNSET1 = 5, SUNSET2 = 6 }

public class EnvironmentManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    [SerializeField] private Material[] skyboxes;
    [SerializeField] private GameObject[] directionnalLights;
    [SerializeField] private float[] lightIntensities;


    [Header("Environment parameters")]
    [Header("Light")]
    [SerializeField] private float zombieLightIntensity;
    [SerializeField] private float nightLightIntensity;

    [Header("Rain")]
    [SerializeField] private float rainFogDensity;
    [SerializeField] private int[] rainAdditions;

    [Header("Fog")]
    [SerializeField] private float normalFogIntensity;
    [SerializeField] private float objectifFogIntensity;

    [SerializeField] private float[] fogIncreases;

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
        int style = (int)envStyleNumber;

        RenderSettings.skybox = skyboxes[style];
        dirLight = Instantiate(directionnalLights[style], Vector3.zero, dirLightRotation).GetComponent<Light>();
        RenderSettings.ambientIntensity = lightIntensities[style];
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
        if (main.GameManager.gameData.gameMode != GameMode.ZOMBIE)
        {
            if (main.GameManager.WaveNumber == 6) // !ZOMBIE
                BedTime(); // Night time when player reaching wave 10
            else if (main.GameManager.gameData.gameWeather == GameWeather.SUN)
            {
                if (main.GameManager.WaveNumber == 4) Sunset1();
                else if (main.GameManager.WaveNumber == 5) Sunset2();
            }
        }

        // # Weather #
        if (main.GameManager.gameData.gameWeather == GameWeather.RAIN) // RAIN
        {
            // Increases the rain according to the difficulty
            IncreaseRain(rainAdditions[(int)main.GameManager.gameData.gameDifficulty]);
        }
        if (main.GameManager.gameData.gameWeather == GameWeather.FOG) // FOG
        {
            // # Difficulties #
            // Increases the fog according to the difficulty
            IncreaseFog(fogIncreases[(int)main.GameManager.gameData.gameDifficulty]);
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

    private void BedTime()
    {
        envStyleNumber = EnvironmentStyle.NIGHT;
        Destroy(dirLight.gameObject);
        ActuEnvironment();
    }
    private void Sunset1()
    {
        envStyleNumber = EnvironmentStyle.SUNSET1;
        Destroy(dirLight.gameObject);
        ActuEnvironment();
    }
    private void Sunset2()
    {
        envStyleNumber = EnvironmentStyle.SUNSET2;
        Destroy(dirLight.gameObject);
        ActuEnvironment();
    }
}
