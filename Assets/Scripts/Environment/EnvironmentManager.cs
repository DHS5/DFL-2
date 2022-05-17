using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Tooltip("Singleton Instance of the GameManager")]
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Material[] skyboxes;
    [SerializeField] private GameObject[] directionnalLights;

    private int renderStyleNumber = 0;

    private GameObject dirLight;

    public void GenerateEnvironment()
    {
        if (gameManager.gameMode == GameMode.ZOMBIE)
        {
            renderStyleNumber = 1;
            RenderSettings.ambientIntensity = 0.4f;
        }
        RenderSettings.skybox = skyboxes[renderStyleNumber];
        dirLight = Instantiate(directionnalLights[renderStyleNumber], new Vector3(0, 0, 0), Quaternion.Euler(50, -30, 0));


        if (gameManager.options.Contains(GameOption.FOG))
        {
            RenderSettings.fog = true;
            //if (gameManager.gameMode == GameMode.OBJECTIF)
            //    RenderSettings.fogDensity = 0.03f;
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
        RenderSettings.skybox = skyboxes[2];
        FindObjectOfType<Light>().gameObject.SetActive(false);
        dirLight = Instantiate(directionnalLights[2], new Vector3(0, 0, 0), Quaternion.Euler(50, -30, 0));
        RenderSettings.ambientIntensity = 0.6f;
    }
}
