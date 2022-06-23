using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;



    [Tooltip("Prefab of the objectif")]
    [SerializeField] private GameObject[] objectifPrefabs;


    [Tooltip("Queue of objectives")]
    public Queue<Objectif> objectives = new Queue<Objectif>();
    [Tooltip("Current objectif for the player to go through")]
    private Objectif currentObjectif;

    [Tooltip("Player script")]
    private Player player;


    [Tooltip("Zones of the field on which to place the objectives")]
    private GameObject[] zones = new GameObject[3];


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    private void Update()
    {
        // Checks if the player misses an objectif
        if (currentObjectif != null && player.transform.position.z > currentObjectif.gameObject.transform.position.z + 5 && !main.GameManager.GameOver)
        {
            Debug.Log("Missed an objectif");
            main.GameManager.GameOver = true;
        }
    }



    // ### Functions ###


    /// <summary>
    /// Gets the field zones
    /// </summary>
    private void GetZones()
    {
        zones[0] = main.FieldManager.field.frontZone;
        zones[1] = main.FieldManager.field.middleZone;
        zones[2] = main.FieldManager.field.endZone;
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
        player = main.PlayerManager.player;


        Vector3 randomPos;
        Vector3 zonePos;
        float xScale;
        float zScale;

        // Gets the field zones
        GetZones();

        for (int i = 0; i < zones.Length; i++)
        {
            // Gets the zones position and scale info
            zonePos = zones[i].transform.position;
            xScale = zones[i].transform.localScale.x/2 - 10;
            zScale = zones[i].transform.localScale.z/2;

            // Gets a random position in the current zone
            randomPos = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale)) + zonePos;

            // Instantiate the objectif
            InstantiateObj(objectifPrefabs[(int) main.GameManager.gameData.gameDifficulty], randomPos);
        }

        // Gets the first objectif
        NextObj();
    }

    public void GenerateObj(int number)
    {
        player = main.PlayerManager.player;


        Vector3 fieldPos = main.FieldManager.field.transform.position;
        float xScale = main.FieldManager.field.fieldZone.transform.localScale.x / 2 - 5;
        float zRange = main.FieldManager.field.fieldZone.transform.localScale.z / (number + 1);
        float xRange = zRange * Mathf.Tan(Mathf.Asin(player.controller.SlowSideSpeed / player.controller.NormalSpeed));
        
        Vector3 randomPos = Vector3.zero;

        for (int i = 1; i < number + 1; i++)
        {
            float x = Mathf.Clamp(Random.Range(xRange / 2, xRange) * (Random.Range(0, 2) == 0 ? -1 : 1) + randomPos.x, -xScale, xScale);
            randomPos = new Vector3(x, 0, zRange * i) + fieldPos;
            InstantiateObj(objectifPrefabs[(int) main.GameManager.gameData.gameDifficulty], randomPos);
        }

        NextObj();
    }

    private void InstantiateObj(GameObject prefab, Vector3 position)
    {
        Objectif obj;

        obj = Instantiate(prefab, position, Quaternion.identity).GetComponent<Objectif>();
        obj.objectifManager = this;
        objectives.Enqueue(obj);
    }
}
