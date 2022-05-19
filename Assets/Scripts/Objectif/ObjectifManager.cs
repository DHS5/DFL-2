using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    [Tooltip("Singleton Instance of the GameManager")]
    [SerializeField] private GameManager gameManager;

    [Tooltip("Game object of the player")]
    [SerializeField] private GameObject player;

    [Tooltip("Prefab of the objectif")]
    [SerializeField] private GameObject[] objectifPrefabs;


    [Tooltip("Queue of objectives")]
    public Queue<Objectif> objectives = new Queue<Objectif>();
    [Tooltip("Current objectif for the player to go through")]
    private Objectif currentObjectif;


    [Tooltip("Zones of the field on which to place the objectives")]
    private GameObject[] zones = new GameObject[3];

    /// <summary>
    /// Gets the field zones
    /// </summary>
    private void GetZones()
    {
        zones[0] = gameManager.currentField.frontZone;
        zones[1] = gameManager.currentField.middleZone;
        zones[2] = gameManager.currentField.endZone;
    }

    /// <summary>
    /// Gets the next objectif
    /// </summary>
    public void NextObj()
    {
        if (objectives.Count > 0) currentObjectif = objectives.Dequeue();
    }


    /// <summary>
    /// Generates the objectives
    /// </summary>
    public void GenerateObj()
    {
        Objectif obj;

        // Gets the field zones
        GetZones();

        for (int i = 0; i < zones.Length; i++)
        {
            // Gets the zones position and scale info
            Vector3 zonePos = zones[i].transform.position;
            float xScale = zones[i].transform.localScale.x/2 - 10;
            float zScale = zones[i].transform.localScale.z/2;

            // Gets a random position in the current zone
            Vector3 randomPos = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale)) + zonePos;

            // Instantiate the objectif
            obj = Instantiate(objectifPrefabs[(int) gameManager.gameDifficulty / 2], randomPos, Quaternion.identity).GetComponent<Objectif>();
            obj.objectifManager = this;
            objectives.Enqueue(obj);
        }

        // Gets the first objectif
        NextObj();
    }


    private void Update()
    {
        // Checks if the player misses an objectif
        if (currentObjectif != null && player.transform.position.z > currentObjectif.gameObject.transform.position.z + 5 && !gameManager.gameOver)
        {
            Debug.Log("Missed an objectif");
            gameManager.gameOver = true;
        }
    }
}
