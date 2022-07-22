using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [Header("Obstacle prefabs list")]
    [Tooltip("Prefabs of the obstacles")]
    [SerializeField] private List<GameObject> obstaclePrefabs = new List<GameObject>();

    [Header("Obstacle parameter")]
    [Tooltip("Max number of obstacles on a single field")]
    [SerializeField] private int obstaclesLimit;


    [Tooltip("List of the active obstacles")]
    private List<GameObject> obstacles = new List<GameObject>();

    [Tooltip("Field zone where the obstacles are placable")]
    private GameObject obstacleZone;

    


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    /// <summary>
    /// Gets the field zone
    /// </summary>
    private void GetZone()
    {
        obstacleZone = main.FieldManager.field.obstacleZone;
    }


    /// <summary>
    /// Generates obstacles all over the field
    /// </summary>
    /// <param name="number">Number of obstacles to generate</param>
    public void GenerateObstacles(int number)
    {
        // Gets the current field zone
        GetZone();
        
        Vector3 randomPos;
        Quaternion randomOrientation;
        Vector3 fieldPos = obstacleZone.transform.position;
        float xScale = obstacleZone.transform.localScale.x / 2;
        float zScale = obstacleZone.transform.localScale.z / 2;

        number = Mathf.Clamp(number, 0, obstaclesLimit);

        for (int i = 0; i < number; i++)
        {
            randomPos = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale)) + fieldPos; // Empêcher de placer sur un objectif
            randomOrientation = Quaternion.Euler(0, Random.Range(0, 180), 0);
            GameObject obs = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], randomPos, randomOrientation);
            obstacles.Add(obs);
        }
    }


    /// <summary>
    /// Destroys the active obstacles
    /// </summary>
    public void DestroyObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
        }
        obstacles.Clear();
    }
}
