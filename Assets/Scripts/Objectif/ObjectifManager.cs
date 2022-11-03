using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;



    [Tooltip("Prefab of the objectif")]
    [SerializeField] private GameObject[] objectifPrefabs;
    [SerializeField] private GameObject[] finalObjectifPrefabs;


    [Tooltip("Queue of objectives")]
    public Queue<Objectif> objectives = new Queue<Objectif>();
    [Tooltip("Current objectif for the player to go through")]
    private Objectif currentObjectif;

    [Tooltip("Player script")]
    private Player player;


    // ### Properties ###
    private Objectif CurrentObjectif
    {
        get { return currentObjectif; }
        set
        {
            currentObjectif = value;
            currentObjectif.gameObject.SetActive(true);
        }
    }



    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    private void Update()
    {
        // Checks if the player misses an objectif
        if (CurrentObjectif != null && player.transform.position.z > CurrentObjectif.gameObject.transform.position.z + 5 && !main.GameManager.GameOver)
        {
            Debug.Log("Missed an objectif");
            main.PlayerManager.player.gameplay.Lose();
        }
    }



    // ### Functions ###


    /// <summary>
    /// Gets the next objectif
    /// </summary>
    public void NextObj()
    {
        if (objectives.Count > 0) CurrentObjectif = objectives.Dequeue();
    }


    
    /// <summary>
    /// Generates the objectives
    /// </summary>
    /// <param name="number">Number of objectives to generate</param>
    public void GenerateObj(int number)
    {
        player = main.PlayerManager.player;

        int diff = (int)main.GameManager.gameData.gameDifficulty;
        float diffMultiplier = 0.5f + diff / 10;
        Vector3 fieldPos = main.FieldManager.field.transform.position;
        float xScale = main.FieldManager.field.fieldZone.transform.localScale.x / 2 - 5;
        float zRange = main.FieldManager.field.fieldZone.transform.localScale.z / (number + 1);
        float xRange = diffMultiplier * zRange * Mathf.Tan(Mathf.Asin(player.controller.playerAtt.SlowSideSpeed / player.controller.playerAtt.NormalSpeed));


        Vector3 randomPos = Vector3.zero;

        float addX, x;
        float min = xRange * ((float)(diff + 1) / (2 + diff));

        for (int i = 1; i < number + 1; i++)
        {
            addX = Random.Range(min, xRange) * (Random.Range(0, 2) == 0 ? -1 : 1);
            if (Mathf.Abs(addX + randomPos.x) > xScale) addX = - addX;
            x = Mathf.Clamp(addX + randomPos.x, -xScale, xScale);
            randomPos = new Vector3(x, 0, zRange * i) + fieldPos;
            InstantiateObj(i != number ? objectifPrefabs[diff] : finalObjectifPrefabs[diff], randomPos, i == 1, i != number);
        }

        NextObj();
    }
    /// <summary>
    /// Generates the objectives
    /// </summary>
    public void GenerateObj()
    {
        GenerateObj(3 + (int)main.GameManager.gameData.gameDifficulty);
    }

    /// <summary>
    /// Instantiate an objectif
    /// </summary>
    /// <param name="prefab">Prefab of the objectif</param>
    /// <param name="position">Position of the objectif</param>
    private void InstantiateObj(GameObject prefab, Vector3 position, bool active, bool rot)
    {
        Objectif obj;

        int rotation = rot ? Random.Range(-75, 75) : 0;

        obj = Instantiate(prefab, position, Quaternion.Euler(0, rotation ,0)).GetComponent<Objectif>();
        obj.objectifManager = this;
        objectives.Enqueue(obj);
        obj.gameObject.SetActive(active);
    }
}
