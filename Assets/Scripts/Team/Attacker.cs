using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attacker : MonoBehaviour
{
    [Tooltip("Team Manager")]
    [HideInInspector] public TeamManager teamManager;


    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    protected AttackerState currentState;

    [HideInInspector] public AttackerType type;


    [HideInInspector] public Player player;
    [HideInInspector] public Enemy target;

    [HideInInspector] public Vector3 destination;

    protected bool gameOver;

    public float reactivity;
    public float positionRadius;
    [Range(0,1)] public float defenseDistMultiplier;

    public float back2PlayerSpeed;
    public float defenseSpeed;


    [HideInInspector] public Vector3 playerPos;
    [HideInInspector] public Vector3 playerDir;

    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 targetDir;

    [HideInInspector] public float playerTargetDist;
    [HideInInspector] public Vector3 player2TargetDir;


    [SerializeField] private float size;
    public float Size
    {
        get { return size; }
        set
        {
            size = value;
            transform.localScale *= size;
        }
    }
    
    [HideInInspector] public bool hasDefender = false;

    private Quaternion lookRot;

    private Vector3 b4StopVelocity;


    public float ProtectionRadius { get { return teamManager.protectionRadius; } }


    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        // Walk
        //if (navMeshAgent.velocity.magnitude > walkLimit && navMeshAgent.velocity.magnitude < runLimit)
        //{
        //    animator.SetFloat("Speed", 1f);
        //    if (target != null) lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
        //    else lookRot = transform.rotation;
        //    navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRot, 5 * Time.deltaTime);
        //}
        //// Run
        //else if (navMeshAgent.velocity.magnitude >= runLimit)
        //{
        //    animator.SetFloat("Speed", 2f);
        //}
        //// Idle
        //else
        //{
        //    animator.SetFloat("Speed", 0f);
        //
        //    if (target != null) lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
        //    else lookRot = transform.rotation;
        //    navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRot, 5 * Time.deltaTime);
        //}
    }



    /// <summary>
    /// Stops the attacker
    /// </summary>
    public void Stop()
    {
        b4StopVelocity = navMeshAgent.velocity;
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
        animator.enabled = false;
    }
    /// <summary>
    /// Resumes the attacker
    /// </summary>
    public void Resume()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = b4StopVelocity;
        animator.enabled = true;
    }

    public void GameOver()
    {
        gameOver = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("GameOver");
    }


    public virtual void ProtectPlayer()
    {
        // ### Player
        // Gets the player's position
        playerPos = player.transform.position;
        // Gets the player direction
        playerDir = player.controller.Velocity.normalized;

        if (target != null)
        {
            // ### Target
            // Gets the player's position
            targetPos = target.transform.position;
            // Gets the player direction
            targetDir = target.GetComponent<NavMeshAgent>().velocity.normalized;

            // Gets the distance between player and target
            playerTargetDist = Vector3.Distance(playerPos, targetPos);
            // Gets the direction from player to target
            player2TargetDir = (targetPos - playerPos).normalized;
        }
    }



    public virtual void TargetEnemy(Enemy enemy)
    {
        teamManager.SuppEnemy(enemy);
        hasDefender = true;
        target = enemy;
    }

    public virtual void UnTarget()
    {
        hasDefender = false;
        teamManager.AddEnemy(target);
    }

    protected virtual void BlockEnemy()
    {

    }


    public virtual Vector3 ClampInZone(Vector3 destination) 
    {
        return Vector3.zero; 
    }

    public virtual bool InZone(Vector3 destination)
    {
        return false;
    }
}
