using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum AttackerPosition { FRONT = 0, LSIDE = 1, RSIDE = 2, BACK = 3 }

[System.Serializable]
public enum AttackerType { GUARD = 0, BLOCKER = 1, PUSHER = 2 }

/// <summary>
/// Manages the team effort of the attackers
/// </summary>
public class TeamManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [Tooltip("Script of the player")]
    private Player player;


    [Tooltip("Attackers's base prefabs")]
    [SerializeField] private GameObject[] attackerBasePrefabs;


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



    [Header("Team caracteristics")]
    public float protectionRadius;
    public float teamReactivity;
    [Range(0,1)] public float directionInterpolation;


    [Header("Limit angles")]
    public float frontAngle;
    public float backAngle;
    public float sideAngleMargin;

    // ### Properties ###
    public Vector3 TeamDir
    {
        get { return Vector3.Slerp(player.controller.Velocity.normalized, player.transform.forward, directionInterpolation); }
    }
    private float TeamReactivity
    {
        get { return 0.04f - (int)main.GameManager.gameData.gameDifficulty * 0.01f; }
    }


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }

    private void Update()
    {
        if (TeamReactivity == 0 && player.gameplay.onField && main.GameManager.GameOn && !main.GameManager.GameOver)
            ProtectPlayer();
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
        foreach (Attacker a in frontAttackers.Union(backAttackers).Union(sideLAttackers).Union(sideRAttackers))
        {
            Destroy(a.gameObject);
        }
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
    private void InstantiateAttacker(AttackerAttributesSO att)
    {
        Vector3 zonePos = main.FieldManager.field.enterZone.transform.position;
        float xScale = main.FieldManager.field.enterZone.transform.localScale.x / 2;
        float zScale = main.FieldManager.field.enterZone.transform.localScale.z / 2;

        Vector3 randomPos = new Vector3(Random.Range(-xScale, xScale), 0, Random.Range(-zScale, zScale)) + zonePos;

        Attacker attacker = Instantiate(attackerBasePrefabs[(int)att.Position], randomPos, Quaternion.identity).GetComponent<Attacker>();
        attacker.player = player;
        attacker.GetAttribute(att);
        attacker.teamManager = this;
        AddAttackerToList(attacker);
    }

    private void AddAttackerToList(Attacker a)
    {
        switch (a.Attribute.Position)
        {
            case AttackerPosition.FRONT:
                frontAttackers.Add(a);
                break;
            case AttackerPosition.BACK:
                backAttackers.Add(a);
                break;
            case AttackerPosition.LSIDE:
                sideLAttackers.Add(a);
                break;
            case AttackerPosition.RSIDE:
                sideRAttackers.Add(a);
                break;
            default:
                break;
        }
    }


    public void TeamCreation()
    {
        player = main.PlayerManager.player;

        for (int i = 0; i < 5 - ((int) main.GameManager.gameData.gameDifficulty / 2); i++)
        {
            InstantiateAttacker(main.GameManager.gameData.team[i]);
        }
    }


    /// <summary>
    /// Begins the player's protection
    /// </summary>
    public void BeginProtection()
    {
        enemies.Clear();
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
                float enemyPlayerAngle = Vector3.Angle(e.transform.position - player.transform.position, TeamDir);
                FindFreeAttackers(enemyPlayerAngle, e.transform.position.x - player.transform.position.x);

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
        if (player.gameplay.onField && main.GameManager.GameOn && !main.GameManager.GameOver && TeamReactivity != 0)
            Invoke(nameof(ProtectPlayer), TeamReactivity);
    }


    private void FindFreeAttackers(float angle, float xDist)
    {
        // Front
        if (angle <= frontAngle)
        {
            freeAttackers = new List<Attacker>(frontAttackers);
            if (angle >= frontAngle - sideAngleMargin)
            {
                foreach (Attacker a in xDist < 0 ? sideLAttackers : sideRAttackers)
                    freeAttackers.Add(a);
            }
        }
        // Back
        else if (angle >= backAngle)
            freeAttackers = new List<Attacker>(backAttackers);
        // L / R Side
        else
            freeAttackers = new List<Attacker>(xDist < 0 ? sideLAttackers : sideRAttackers);
        

        busyAttackers = new List<Attacker>(freeAttackers);

        foreach (Attacker a in busyAttackers)
        {
            if (a.hasDefender)
            {
                freeAttackers.Remove(a);
            }
        }
    }
}
