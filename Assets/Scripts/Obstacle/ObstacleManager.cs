using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [Tooltip("Prefabs of the obstacles")]
    [SerializeField] private List<GameObject> obstaclePrefabs = new List<GameObject>();

    [Tooltip("List of the active obstacles")]
    private List<GameObject> obstacles = new List<GameObject>();

    [Tooltip("Field zone where the obstacles are placable")]
    private GameObject fieldZone;

    [Tooltip("Max number of obstacles placable on a single field")]
    [SerializeField] private int obstaclesLimit;


    private void Start()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    /// <summary>
    /// Gets the field zone
    /// </summary>
    private void GetZone()
    {
        fieldZone = main.FieldManager.fieldScript.fieldZone;
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
        Vector3 fieldPos = fieldZone.transform.position;
        float xScale = fieldZone.transform.localScale.x / 2;
        float zScale = fieldZone.transform.localScale.z / 2;

        number = Mathf.Clamp(number, 0, obstaclesLimit);

        for (int i = 0; i < number; i++)
        {
            randomPos = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale)) + fieldPos;
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
