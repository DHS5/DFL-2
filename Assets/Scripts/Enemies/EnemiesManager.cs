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

    [Header("Enemies")]
    [SerializeField] private EnemyContainerSO enemyContainer;
    [SerializeField] private GameObject[] basePrefabs;

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
    [HideInInspector] public List<Enemy> enemies;


    readonly Quaternion enemyRot = Quaternion.Euler(0, 180, 0);
    readonly int sleepingZProportion = 5;


    // Zones of the field
    private GameObject obstacleZone;
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
            if (enemies[i] != null)
                Destroy(enemies[i].gameObject);
        enemies.Clear();
    }


    /// <summary>
    /// Stops all enemies
    /// </summary>
    public void StopEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                enemies[i].Stop();
        }
    }
    /// <summary>
    /// Resumes all enemies
    /// </summary>
    public void ResumeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                enemies[i].Resume();
        }
    }

    public void GameOver()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                enemies[i].GameOver();
        }
    }

    /// <summary>
    /// Starts the chase for all the enemies on the field
    /// </summary>
    public void BeginChase()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                enemies[i].ChasePlayer();
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

        obstacleZone = field.obstacleZone;
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
    private void CreateEnemy<T>(T defAtt, Vector3 pos, AudioClip[] audios) where T : EnemyAttributesSO
    {        
        // Game Object of the enemy
        Enemy enemy;

        // Instantiate the new enemy
        enemy = Instantiate(basePrefabs[defAtt.Type], pos, enemyRot).GetComponent<Enemy>();
        enemy.GetAttribute(defAtt);

        // Gives the enemy his body and a semi-random size
        enemy.enemy = enemy.gameObject;
        if (audios != null && audios.Length > 0)
        {
            enemy.audioSource.clip = audios[Random.Range(0, audios.Length)];
            enemy.audioSource.PlayDelayed(Random.Range(0f, 1f));
        }

        // Fill the enemies list of the field
        enemies.Add(enemy);
    }

    private T GetRandomEnemy<T>(T[] enemies) where T : EnemyAttributesSO
    {
        GameData gd = main.GameManager.gameData;

        // Clamps the level so it doesn't get out of the enemyPrefabs length
        int maxLevel = Mathf.Clamp(main.GameManager.WaveNumber, 1, enemies.Length);
        int minLevel = Mathf.Clamp(main.GameManager.WaveNumber - gd.gameEnemiesRange, 0, enemies.Length - 1);

        return enemies[Random.Range(minLevel, maxLevel)];
    }

    private Vector3 GetRandomPos(Vector3 pos, float xScale, float zScale)
    {
        return pos + new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale));
    }


    /// <summary>
    /// Generates a defender wave
    /// (11 defenders, 5 in the center, 3 on each side)
    /// </summary>
    private void DefendersWave()
    {
        DefenderAttributesSO[] wingmanPrefabs = enemyContainer.GetArrays((int)main.GameManager.gameData.gameDifficulty).wingmen;
        DefenderAttributesSO[] linemanPrefabs = enemyContainer.GetArrays((int)main.GameManager.gameData.gameDifficulty).linemen;

        // Spawn in the center zone
        Vector3 center = centerZone.transform.position;
        float xScale = centerZone.transform.localScale.x / 2;
        float zScale = centerZone.transform.localScale.z / 2;
        // 5 Linemen in the center
        for (int i = 0; i < 5; i++)
        {
            CreateEnemy(GetRandomEnemy(linemanPrefabs), GetRandomPos(center, xScale, zScale), defenderAudios);
        }

        // Spawn in the left zone
        Vector3 left = leftZone.transform.position;
        xScale = leftZone.transform.localScale.x / 2;
        zScale = leftZone.transform.localScale.z / 2;
        // 3 Wingmen on the left
        for (int i = 0; i < 3; i++)
        {
            CreateEnemy(GetRandomEnemy(wingmanPrefabs), GetRandomPos(left, xScale, zScale), defenderAudios);
        }

        // Spawn in the right zone
        Vector3 right = rightZone.transform.position;
        xScale = rightZone.transform.localScale.x / 2;
        zScale = rightZone.transform.localScale.z / 2;
        // 3 Wingmen on the right
        for (int i = 0; i < 3; i++)
        {
            CreateEnemy(GetRandomEnemy(wingmanPrefabs), GetRandomPos(right, xScale, zScale),  defenderAudios);
        }

    }

    /// <summary>
    /// Generates a zombie wave
    /// </summary>
    private void ZombiesWave()
    {
        ZombieAttributesSO[] classicZPrefabs = enemyContainer.GetZArrays((int)main.GameManager.gameData.gameDifficulty).classic;
        ZombieAttributesSO[] sleepingZPrefabs = enemyContainer.GetZArrays((int)main.GameManager.gameData.gameDifficulty).sleeping;

        Vector3 field = obstacleZone.transform.position;
        float xScale = obstacleZone.transform.localScale.x / 2;
        float zScale = obstacleZone.transform.localScale.z / 2;
        int r;
        // Spawn on the whole field
        for (int i = 0; i < 50 + (3 + (int) main.GameManager.gameData.gameDifficulty) * (main.GameManager.WaveNumber + (int)main.GameManager.gameData.gameDifficulty) ; i++)
        {
            r = Random.Range(1, sleepingZProportion + 1);
            ZombieAttributesSO enemy;
            if (r != 1) enemy = GetRandomEnemy(classicZPrefabs);
            else enemy = GetRandomEnemy(sleepingZPrefabs);

            CreateEnemy(enemy, GetRandomPos(field, xScale, zScale), zombieAudios);
        }
    }


    /// <summary>
    /// Generates a drill wave
    /// </summary>
    private void DrillWave()
    {
        if (main.GameManager.gameData.gameDrill == GameDrill.ONEVONE)
        {
            CreateEnemy(main.GameManager.gameData.enemy, main.FieldManager.field.OneVOneEnemyPos, defenderAudios);
        }
    }
}

