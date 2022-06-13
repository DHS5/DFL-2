using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract class defining the base methods and properties of an enemy
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [Tooltip("Game Object of the enemy")]
    [HideInInspector] public GameObject enemy;

    [Tooltip("Nav Mesh Agent of the enemy")]
    protected NavMeshAgent navMeshAgent;

    [HideInInspector] public AudioSource audioSource;

    protected Animator animator;

    protected EnemyState currentState;



    [Tooltip("Game Object of the player")]
    [HideInInspector] public GameObject player;

    protected Player playerScript;

    [Tooltip("PlayerGameplay component of the player")]
    protected PlayerGameplay playerG;

    [Tooltip("PlayerController component of the player")]
    protected PlayerController playerC;



    [Header("Behaviour parameters")]
    [Tooltip("Level of intelligence of the enemy (anticipation of the future position)")]
    [Range(0,1)] public float intelligence;
    [Tooltip("Level of reactivity of the enemy (time between new destination's settings)")]
    public float reactivity;
    [Tooltip("")]
    public float anticipation;
    [Tooltip("Level of precision of the enemy (precision in the positionning in degrees)")]
    public float precision;


    [Header("Z distances")]
    [Tooltip("")]
    public float waitDist;

    [Header("Raw distances")]
    [Tooltip("")]
    public float attackDist;


    [Header("Physical parameters")]
    [Tooltip("Attack speed of the enemy")]
    public float attackSpeed;

    [Tooltip("Size of the enemy")]
    [SerializeField] protected float size;


    [Header("Wing Man caracteristics")]
    public bool patient;

    public float patience;
    [Tooltip("Distance before chasing (Raw distance)")]
    public float chaseDist;



    [Header("Line Man caracteristics")]
    [Tooltip("Distance before quitting the positionning state (Z - distance)")]
    public float positionningDist;


    public float Size
    {
        get { return size; }
        set
        {
            size = value;
            enemy.transform.localScale *= size;
        }
    }

    [Tooltip("Position of the player")]
    [HideInInspector] public Vector3 playerPosition;
    [Tooltip("Look direction of the player")]
    [HideInInspector] public Vector3 playerLookDirection;
    [Tooltip("Velocity of the player")]
    [HideInInspector] public Vector3 playerVelocity;
    [Tooltip("Speed of the player")]
    [HideInInspector] public float playerSpeed;
    [Tooltip("Direction from the enemy to the player")]
    [HideInInspector] public Vector3 toPlayerDirection;
    [Tooltip("Angle between the enemy and the player")]
    [HideInInspector] public float toPlayerAngle;
    [Tooltip("Distance between the enemy and the player")]
    [HideInInspector] public float rawDistance;
    [Tooltip("Distance between the enemy and the player on the X-Axis only")]
    [HideInInspector] public float xDistance;
    [Tooltip("Distance between the enemy and the player on the Z-Axis only")]
    [HideInInspector] public float zDistance;
    [Tooltip("Whether the player is on the field")]
    [HideInInspector] public bool playerOnField;

    [Tooltip("Destination of the enemy (on the nav mesh)")]
    [HideInInspector] public Vector3 destination;



    private Vector3 b4StopVelocity;

    //public string state;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();

        playerScript = FindObjectOfType<Player>();
        playerG = playerScript.gameplay;
        playerC = playerScript.controller;
    }



    // ### Functions ###

    /// <summary>
    /// Stops the enemy
    /// </summary>
    public void Stop()
    {
        b4StopVelocity = navMeshAgent.velocity;
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
        animator.enabled = false;
    }
    /// <summary>
    /// Resumes the enemy
    /// </summary>
    public void Resume()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.velocity = b4StopVelocity;
        animator.enabled = true;
    }


    /// <summary>
    /// Virtual base class giving the children the information concerning the player
    /// </summary>
    public virtual void ChasePlayer()
    {
        if (playerG.isVisible)
        {
            // Gets the player's position
            playerPosition = player.transform.position;
            // Gets the player's look direction
            playerLookDirection = playerScript.activeBody.transform.forward.normalized;
            // Gets the player's velocity
            playerVelocity = playerC.Velocity.normalized;
            // Gets the player's speed
            playerSpeed = playerC.Speed;
            // Gets the direction to the player
            toPlayerDirection = (playerPosition - transform.position).normalized;
            // Gets the angle between the enemy's and the player's directions
            toPlayerAngle = Vector3.Angle(transform.forward, toPlayerDirection);
            // Gets the distance between the player and the enemy
            rawDistance = Vector3.Distance(playerPosition, transform.position);
            // Gets the distance between the player and the enemy on the Z-Axis
            xDistance = Mathf.Abs(transform.position.x - playerPosition.x);
            // Gets the distance between the player and the enemy on the Z-Axis
            zDistance = transform.position.z - playerPosition.z;

            playerOnField = playerG.onField;
        }
    }
}
