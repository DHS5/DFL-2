using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum AttackerType { FRONT = 0, LSIDE = 1, RSIDE = 2, BACK = 3}

/// <summary>
/// Manages the team effort of the attackers
/// </summary>
public class TeamManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [Tooltip("Script of the player")]
    private Player player;


    [Tooltip("List of the attackers's prefabs")]
    [SerializeField] private GameObject[] attackersPrefabs;


    [Tooltip("List of the team's front attackers")]
    private List<Attacker> frontAttackers = new List<Attacker>();
    [Tooltip("List of the team's left side attackers")]
    private List<Attacker> sideLAttackers = new List<Attacker>();
    [Tooltip("List of the team's right side attackers")]
    private List<Attacker> sideRAttackers = new List<Attacker>();
    [Tooltip("List of the team's back attackers")]
    private List<Attacker> backAttackers = new List<Attacker>();

    private List<Attacker> freeAttackers = new List<Attacker>();
    private List<Attacker> busyAttackers = new List<Attacker>();

    [Tooltip("List of the enemies currently not taken care of")]
    [HideInInspector] public List<Enemy> enemies;

    private List<Enemy> enemiesToSupp = new List<Enemy>();
    private List<Enemy> enemiesToAdd = new List<Enemy>();


    public float protectionRadius;
    [SerializeField] private float teamReactivity;


    readonly float sizeMultiplier = 0.1f;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    /// <summary>
    /// Stops all the attackers
    /// </summary>
    public void StopAttackers()
    {
        foreach (Attacker a in frontAttackers.Union(backAttackers).Union(sideLAttackers).Union(sideRAttackers))
            a.Stop();
    }

    /// <summary>
    /// Resume all the attackers
    /// </summary>
    public void ResumeAttackers()
    {
        foreach (Attacker a in frontAttackers.Union(backAttackers).Union(sideLAttackers).Union(sideRAttackers))
            a.Resume();
    }

    public void GameOver()
    {
        foreach (Attacker a in frontAttackers.Union(backAttackers).Union(sideLAttackers).Union(sideRAttackers))
            a.GameOver();
    }




    /// <summary>
    /// Add an enemy to the enemies list
    /// </summary>
    /// <param name="enemy">Enemy to add to the list</param>
    public void AddEnemy(Enemy enemy)
    {
        enemiesToAdd.Add(enemy);
    }

    /// <summary>
    /// Supp an enemy from the enemies list
    /// </summary>
    /// <param name="enemy">Enemy to supp from the list</param>
    public void SuppEnemy(Enemy enemy)
    {
        enemiesToSupp.Add(enemy);
    }

    private void ActuEnemies()
    {
        foreach (Enemy e in enemiesToSupp)
        {
            enemies.Remove(e);
        }
        foreach (Enemy e in enemiesToAdd)
        {
            enemies.Add(e);
        }
    }

    public void ClearAttackers()
    {
        // Attackers
        foreach (Attacker a in frontAttackers.Union(backAttackers).Union(sideLAttackers).Union(sideRAttackers)) Destroy(a);
        frontAttackers.Clear();
        backAttackers.Clear();
        sideRAttackers.Clear();
        sideLAttackers.Clear();
        // Enemies
        enemies.Clear();
        enemiesToAdd.Clear();
        enemiesToSupp.Clear();
    }


    /// <summary>
    /// Instantiate an attacker from the attackers prefab list with a semi-random position
    /// </summary>
    private void InstantiateAttacker()
    {
        Vector3 zonePos = main.FieldManager.field.enterZone.transform.position;
        float xScale = main.FieldManager.field.enterZone.transform.localScale.x / 2;
        float zScale = main.FieldManager.field.enterZone.transform.localScale.z / 2;

        Vector3 randomPos = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale)) + zonePos;

        Attacker attacker = Instantiate(attackersPrefabs[0], randomPos, Quaternion.identity).GetComponent<Attacker>();
        attacker.teamManager = this;
        attacker.player = player;
        attacker.Size *= Random.Range(1 - sizeMultiplier, 1 + sizeMultiplier);
        AddAttackerToList(attacker);
    }

    private void AddAttackerToList(Attacker a)
    {
        switch (a.type)
        {
            case AttackerType.FRONT:
                frontAttackers.Add(a);
                break;
            case AttackerType.BACK:
                backAttackers.Add(a);
                break;
            case AttackerType.LSIDE:
                sideLAttackers.Add(a);
                break;
            case AttackerType.RSIDE:
                sideRAttackers.Add(a);
                break;
            default:
                break;
        }
    }


    public void TeamCreation()
    {
        player = main.PlayerManager.player;

        for (int i = 0; i < 5 - (int) main.GameManager.gameData.gameDifficulty / 2; i++)
        {
            InstantiateAttacker();
        }

        enemies = new List<Enemy>(main.EnemiesManager.enemies);
    }


    /// <summary>
    /// Begins the player's protection
    /// </summary>
    public void BeginProtection()
    {
        enemies = new List<Enemy>(main.EnemiesManager.enemies);

        ProtectPlayer();

        foreach (Attacker a in frontAttackers.Union(backAttackers).Union(sideLAttackers).Union(sideRAttackers))
            a.ProtectPlayer();
    }

    private void ProtectPlayer()
    {
        foreach (Enemy e in enemies)
        {
            float enemyPlayerDist = Vector3.Distance(e.transform.position, player.transform.position);

            if (enemyPlayerDist < protectionRadius)
            {
                float enemyPlayerAngle = Vector3.Angle(player.transform.position - e.transform.position, player.controller.Velocity.normalized);
                FindFreeAttackers(enemyPlayerAngle);

                Attacker betterAttacker = null;

                if (freeAttackers.Count > 0)
                {
                    float minDist = float.PositiveInfinity;
                    foreach (Attacker a in freeAttackers)
                    {
                        float dist = Vector3.Distance(a.transform.position, e.transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            betterAttacker = a;
                        }
                    }
                    betterAttacker.TargetEnemy(e);
                    freeAttackers.Clear();
                }
                else
                {
                    float maxDist = enemyPlayerDist;
                    float targetDist;
                    foreach (Attacker a in busyAttackers)
                    {
                        targetDist = Vector3.Distance(a.target.transform.position, player.transform.position);
                        if (targetDist > maxDist)
                        {
                            maxDist = targetDist;
                            betterAttacker = a;
                        }
                    }
                    if (betterAttacker != null) { betterAttacker.TargetEnemy(e); }
                }
            }
        }
        ActuEnemies();
        if (player.gameplay.onField)
            Invoke(nameof(ProtectPlayer), teamReactivity);
    }


    private void FindFreeAttackers(float angle)
    {
        // Front
        if (Mathf.Abs(angle) <= 45)
            freeAttackers = new List<Attacker>(frontAttackers);
        // L Side
        else if (angle < -45 && angle > -135)
            freeAttackers = new List<Attacker>(sideLAttackers);
        // R Side
        else if (angle > 45 && angle < 135)
            freeAttackers = new List<Attacker>(sideRAttackers);
        // Back
        else
            freeAttackers = new List<Attacker>(backAttackers);

        busyAttackers = new List<Attacker>(freeAttackers);

        foreach (Attacker a in freeAttackers)
        {
            if (a.hasDefender)
            {
                freeAttackers.Remove(a);
            }
        }
    }
}
