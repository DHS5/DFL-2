using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackerType { FRONT = 0, SIDE = 1, BACK = 2}

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


    [Tooltip("List of the team's attackers currently free")]
    private List<Attacker> freeAttackers = new List<Attacker>();
    [Tooltip("List of the team's attackers currently busy")]
    private List<Attacker> busyAttackers = new List<Attacker>();
    [Tooltip("List of the enemies currently not taken care of")]
    [HideInInspector] public List<Enemy> enemies;

    private List<Enemy> enemiesToSupp = new List<Enemy>();
    private List<Enemy> enemiesToAdd = new List<Enemy>();


    public float protectionRadius;

    [SerializeField] private float teamReactivity; // idem


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }

    private void Start()
    {
        player = main.PlayerManager.player;
    }


    // ### Functions ###


    /// <summary>
    /// Stops all the attackers
    /// </summary>
    public void StopAttackers()
    {
        foreach (Attacker a in freeAttackers)
            a.Stop();

        foreach (Attacker a in busyAttackers)
            a.Stop();
    }

    /// <summary>
    /// Resume all the attackers
    /// </summary>
    public void ResumeAttackers()
    {
        foreach (Attacker a in freeAttackers)
            a.Resume();

        foreach (Attacker a in busyAttackers)
            a.Resume();
    }

    public void GameOver()
    {

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

    public void FreeAttacker(Attacker a)
    {
        busyAttackers.Remove(a);
        freeAttackers.Add(a);
    }

    public void ClearAttackers()
    {
        foreach (Attacker a in freeAttackers) Destroy(a);
        foreach (Attacker a in busyAttackers) Destroy(a);
        freeAttackers.Clear();
        busyAttackers.Clear();
        enemies.Clear();
        enemiesToAdd.Clear();
        enemiesToSupp.Clear();
    }


    /// <summary>
    /// Instantiate an attacker from the attackers prefab list with a semi-random position
    /// </summary>
    private void InstantiateAttacker()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 randomPos = new Vector3(Random.Range(-protectionRadius / 2, protectionRadius / 2), 0, Random.Range(10, protectionRadius / 2)) + playerPos;
        Attacker attacker = Instantiate(attackersPrefabs[0], randomPos, Quaternion.identity).GetComponent<Attacker>();
        attacker.teamManager = this;
        attacker.player = player;
        attacker.Size *= Random.Range(0.9f, 1.1f);
        freeAttackers.Add(attacker);
    }


    public void TeamCreation()
    {
        for (int i = 0; i < 5 - (int) main.GameManager.gameData.gameDifficulty / 2; i++)
        {
            InstantiateAttacker();
        }
    }


    /// <summary>
    /// Begins the player's protection
    /// </summary>
    public void BeginProtection()
    {
        enemies = new List<Enemy>(main.EnemiesManager.enemies);

        ProtectPlayer();
    }

    private void ProtectPlayer()
    {
        foreach (Enemy e in enemies)
        {
            float toPlayerAngle = Vector3.Angle(player.transform.forward, e.transform.position - player.transform.position);
            if (Vector3.Distance(e.transform.position, player.transform.position) < protectionRadius * ( 1 - toPlayerAngle/270))
            {
                if (freeAttackers.Count > 0)
                {
                    float minDist = float.PositiveInfinity;
                    Attacker betterAttacker = null;
                    foreach (Attacker a in freeAttackers)
                    {
                        if (Vector3.Distance(a.transform.position, e.transform.position) < minDist)
                        {
                            minDist = Vector3.Distance(a.transform.position, e.transform.position);
                            betterAttacker = a;
                        }
                    }
                    betterAttacker.TargetEnemy(e);
                    busyAttackers.Add(betterAttacker);
                    freeAttackers.Remove(betterAttacker);
                }
                else
                {
                    float minDist = protectionRadius*2;
                    Attacker betterAttacker = null;
                    foreach (Attacker a in busyAttackers)
                    {
                        if (Vector3.Distance(a.GetComponent<Attacker>().target.transform.position, player.transform.position) > protectionRadius &&
                            Vector3.Distance(a.transform.position, e.transform.position) < minDist)
                        {
                            minDist = Vector3.Distance(a.transform.position, e.transform.position);
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

}
