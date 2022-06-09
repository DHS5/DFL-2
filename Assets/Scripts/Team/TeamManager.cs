using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the team effort of the attackers
/// </summary>
public class TeamManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [Tooltip("GameObject of the player")]
    private GameObject player;


    [Tooltip("List of the attackers's prefabs")]
    [SerializeField] private GameObject[] attackersPrefabs;


    [Tooltip("List of the team's attackers currently free")]
    private List<Attackers> freeAttackers = new List<Attackers>();
    [Tooltip("List of the team's attackers currently busy")]
    private List<Attackers> busyAttackers = new List<Attackers>();
    [Tooltip("List of the enemies currently not taken care of")]
    [HideInInspector] public List<GameObject> enemies;

    private List<GameObject> enemiesToSupp = new List<GameObject>();
    private List<GameObject> enemiesToAdd = new List<GameObject>();


    [SerializeField] private float playerProtectionRadius; // A choisir pour chaque attacker

    [SerializeField] private float teamReactivity; // idem


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }

    private void Start()
    {
        player = main.PlayerManager.playerObject;
    }


    /// <summary>
    /// Stops all the attackers
    /// </summary>
    public void StopAttackers()
    {
        foreach (Attackers a in freeAttackers)
            a.Stop();

        foreach (Attackers a in busyAttackers)
            a.Stop();
    }

    /// <summary>
    /// Resume all the attackers
    /// </summary>
    public void ResumeAttackers()
    {
        foreach (Attackers a in freeAttackers)
            a.Resume();

        foreach (Attackers a in busyAttackers)
            a.Resume();
    }


    /// <summary>
    /// Add an enemy to the enemies list
    /// </summary>
    /// <param name="enemy">Enemy to add to the list</param>
    public void AddEnemy(GameObject enemy)
    {
        enemiesToAdd.Add(enemy);
    }

    /// <summary>
    /// Supp an enemy from the enemies list
    /// </summary>
    /// <param name="enemy">Enemy to supp from the list</param>
    public void SuppEnemy(GameObject enemy)
    {
        enemiesToSupp.Add(enemy);
    }

    private void ActuEnemies()
    {
        foreach (GameObject e in enemiesToSupp)
        {
            enemies.Remove(e);
        }
        foreach (GameObject e in enemiesToAdd)
        {
            enemies.Add(e);
        }
    }

    public void FreeAttacker(Attackers a)
    {
        busyAttackers.Remove(a);
        freeAttackers.Add(a);
    }

    public void ClearAttackers()
    {
        foreach (Attackers a in freeAttackers) Destroy(a);
        foreach (Attackers a in busyAttackers) Destroy(a);
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
        Vector3 randomPos = new Vector3(Random.Range(-playerProtectionRadius / 2, playerProtectionRadius / 2), 0, Random.Range(10, playerProtectionRadius / 2)) + playerPos;
        Attackers attacker = Instantiate(attackersPrefabs[0], randomPos, Quaternion.identity).GetComponent<Attackers>();
        attacker.teamManager = this;
        attacker.player = player;
        attacker.playerProtectionRadius = playerProtectionRadius;
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
        enemies = new List<GameObject>(main.EnemiesManager.enemies);

        ProtectPlayer();
    }

    private void ProtectPlayer()
    {
        foreach (GameObject e in enemies)
        {
            float toPlayerAngle = Vector3.Angle(player.transform.forward, e.transform.position - player.transform.position);
            if (Vector3.Distance(e.transform.position, player.transform.position) < playerProtectionRadius * ( 1 - toPlayerAngle/270))
            {
                if (freeAttackers.Count > 0)
                {
                    float minDist = float.PositiveInfinity;
                    Attackers betterAttacker = null;
                    foreach (Attackers a in freeAttackers)
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
                    float minDist = playerProtectionRadius*2;
                    Attackers betterAttacker = null;
                    foreach (Attackers a in busyAttackers)
                    {
                        if (Vector3.Distance(a.GetComponent<Attackers>().target.transform.position, player.transform.position) > playerProtectionRadius &&
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
        if (player.GetComponent<PlayerGameplay>().onField)
            Invoke(nameof(ProtectPlayer), teamReactivity);
    }

}
