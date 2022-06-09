using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the semi-random spawn position of the enemies
/// </summary>
public class EnemiesManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;



    [Header("Defenders")]
    [Header("Enemy's prefab lists")]
    [Tooltip("List of the Wingmen's prefabs")]
    [SerializeField] private GameObject[] wingmenPrefabs;
    [Tooltip("List of the Bigmen's prefabs")]
    [SerializeField] private GameObject[] linemenPrefabs;
    [Tooltip("")]
    [SerializeField] private AudioClip[] defenderAudios;

    [Header("Zombies")]
    [Tooltip("List of the classic zombie's prefabs")]
    [SerializeField] private GameObject[] classicZPrefabs;
    [Tooltip("List of the sleeping zombie's prefabs")]
    [SerializeField] private GameObject[] sleepingZPrefabs;
    [Tooltip("")]
    [SerializeField] private AudioClip[] zombieAudios;


    [Tooltip("List of the enemies on the field")]
    [HideInInspector] public List<GameObject> enemies;


    // Zones of the field
    private GameObject fieldZone;
    private GameObject centerZone;
    private GameObject leftZone;
    private GameObject rightZone;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    /// <summary>
    /// Destroys all the enemies on the field
    /// </summary>
    public void SuppEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i]);
        enemies.Clear();
    }


    /// <summary>
    /// Stops all enemies
    /// </summary>
    public void StopEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().Stop();
        }
    }
    /// <summary>
    /// Resumes all enemies
    /// </summary>
    public void ResumeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().Resume();
        }
    }

    /// <summary>
    /// Starts the chase for all the enemies on the field
    /// </summary>
    public void BeginChase()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().ChasePlayer();
        }
    }


    /// <summary>
    /// Generates an enemy wave given the mode
    /// </summary>
    public void EnemyWave()
    {
        // Gets the spawning zones
        GetZones();
        
        // Generates the enemy wave given the mode
        switch (main.GameManager.gameData.gameMode)
        {
            case GameMode.DEFENDERS:
                DefendersWave();
                break;
            case GameMode.TEAM:
                DefendersWave();
                break;
            case GameMode.ZOMBIE:
                ZombiesWave();
                break;
            case GameMode.DRILL:
                DrillWave();
                break;
            default:
                Debug.Log("No mode");
                break;
        }
    }

    /// <summary>
    /// Gets the field's zones
    /// </summary>
    private void GetZones()
    {
        Field field = main.FieldManager.field;
        
        fieldZone = field.fieldZone;
        centerZone = field.centerZone;
        leftZone = field.leftZone;
        rightZone = field.rightZone;
    }

    /// <summary>
    /// Instantiate an enemy at a semi-random position and size
    /// </summary>
    /// <param name="enemyPrefabs">List of enemy prefabs (category)</param>
    /// <param name="pos">Position of the zone in which create the enemy</param>
    /// <param name="xScale">X scale of the zone</param>
    /// <param name="zScale">Z scale of the zone</param>
    /// <param name="sizeMultiplier">Size multiplier with which create the enemy</param>
    private void CreateEnemy(GameObject[] enemyPrefabs, Vector3 pos, float xScale, float zScale, float sizeMultiplier, AudioClip[] audios)
    {
        GameData gd = main.GameManager.gameData;
        
        // Game Object of the enemy
        Enemy enemy;

        // Clamps the level so it doesn't get out of the enemyPrefabs length
        int maxLevel = Mathf.Clamp(main.GameManager.WaveNumber + (int)gd.gameDifficulty, (int)gd.gameDifficulty, enemyPrefabs.Length);
        int minLevel = Mathf.Clamp(main.GameManager.WaveNumber + (int)gd.gameDifficulty - gd.gameEnemiesRange, (int)gd.gameDifficulty, enemyPrefabs.Length - 1);

        // Gets a random position and instantiate the new enemy
        Vector3 randomPosition = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale));
        enemy = Instantiate(enemyPrefabs[Random.Range(minLevel, maxLevel)], pos + randomPosition, Quaternion.Euler(0, 180, 0)).GetComponent<Enemy>();

        // Gives the enemy his body and a semi-random size
        enemy.enemy = enemy.gameObject;
        enemy.player = main.PlayerManager.playerObject;
        enemy.Size *= Random.Range(1 - sizeMultiplier, 1 + sizeMultiplier);
        if (audios != null && audios.Length > 0)
        {
            enemy.audioSource.clip = audios[Random.Range(0, audios.Length)];
            enemy.audioSource.PlayDelayed(Random.Range(0f, 1f));
        }

        // Fill the enemies list of the field
        enemies.Add(enemy.gameObject);
    }


    /// <summary>
    /// Generates a defender wave
    /// (11 defenders, 5 in the center, 3 on each side)
    /// </summary>
    private void DefendersWave()
    {        
        // Spawn in the center zone
        Vector3 center = centerZone.transform.position;
        float xScale = centerZone.transform.localScale.x / 2;
        float zScale = centerZone.transform.localScale.z / 2;
        // 5 Linemen in the center
        for (int i = 0; i < 5; i++)
        {
            CreateEnemy(linemenPrefabs, center, xScale, zScale, 0.1f, defenderAudios);
        }

        // Spawn in the left zone
        Vector3 left = leftZone.transform.position;
        xScale = leftZone.transform.localScale.x / 2;
        zScale = leftZone.transform.localScale.z / 2;
        // 3 Wingmen on the left
        for (int i = 0; i < 3; i++)
        {
            CreateEnemy(wingmenPrefabs, left, xScale, zScale, 0.1f, defenderAudios);
        }

        // Spawn in the right zone
        Vector3 right = rightZone.transform.position;
        xScale = rightZone.transform.localScale.x / 2;
        zScale = rightZone.transform.localScale.z / 2;
        // 3 Wingmen on the right
        for (int i = 0; i < 3; i++)
        {
            CreateEnemy(wingmenPrefabs, right, xScale, zScale, 0.1f, defenderAudios);
        }

    }

    /// <summary>
    /// Generates a zombie wave
    /// </summary>
    private void ZombiesWave()
    {
        Vector3 field = fieldZone.transform.position;
        float xScale = fieldZone.transform.localScale.x / 2;
        float zScale = fieldZone.transform.localScale.z / 2;
        int r;
        // Spawn on the whole field
        for (int i = 0; i < 50 + (3 + (int) main.GameManager.gameData.gameDifficulty) * (main.GameManager.WaveNumber + (int)main.GameManager.gameData.gameDifficulty) ; i++)
        {
            r = Random.Range(1, 3);
            if (r == 1) CreateEnemy(classicZPrefabs, field, xScale, zScale, 0.1f, zombieAudios);
            else if (r == 2) CreateEnemy(sleepingZPrefabs, field, xScale, zScale, 0.1f, zombieAudios);
        }
    }


    /// <summary>
    /// Generates a drill wave
    /// </summary>
    private void DrillWave()
    {

    }
}
