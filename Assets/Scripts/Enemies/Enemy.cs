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
    [Tooltip("Level of patience of the enemy")]
    public float patience;

    [Header("Z distances")]
    [Tooltip("")]
    public float waitDist;
    [Tooltip("")]
    public float positionningDist;

    [Header("Raw distances")]
    [Tooltip("")]
    public float chaseDist;
    [Tooltip("")]
    public float attackDist;


    [Header("Physical parameters")]
    [Tooltip("Attack speed of the enemy")]
    [SerializeField] public float attackSpeed;

    [Tooltip("Size of the enemy")]
    [SerializeField] protected float size;
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
    [Tooltip("Direction of the player")]
    [HideInInspector] public Vector3 playerDirection;
    [Tooltip("Speed of the player")]
    [HideInInspector] public float playerSpeed;
    [Tooltip("Direction from the enemy to the player")]
    [HideInInspector] public Vector3 toPlayerDirection;
    [Tooltip("Angle between the enemy and the player")]
    [HideInInspector] public float toPlayerAngle;
    [Tooltip("Distance between the enemy and the player")]
    [HideInInspector] public float rawDistance;
    [Tooltip("Distance between the enemy and the player on the Z-Axis only")]
    [HideInInspector] public float zDistance;

    [Tooltip("Destination of the enemy (on the nav mesh)")]
    [HideInInspector] public Vector3 destination;



    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        playerG = player.GetComponent<PlayerGameplay>();
        playerC = player.GetComponent<PlayerController>();   
    }


    // ### Functions ###

    /// <summary>
    /// Stops the enemy
    /// </summary>
    public void Stop()
    {
        navMeshAgent.isStopped = true;
        animator.enabled = false;
    }
    /// <summary>
    /// Resumes the enemy
    /// </summary>
    public void Resume()
    {
        navMeshAgent.isStopped = false;
        animator.enabled = true;
    }


    /// <summary>
    /// Virtual base class giving the children the information concerning the player
    /// </summary>
    public virtual void ChasePlayer()
    {
        if (player.GetComponent<PlayerGameplay>().isVisible)
        {
            // Gets the player's position
            playerPosition = player.transform.position;
            // Gets the player's direction
            playerDirection = player.transform.forward.normalized;
            // Gets the player's speed
            playerSpeed = playerC.Speed;
            // Gets the direction to the player
            toPlayerDirection = (playerPosition - transform.position).normalized;
            // Gets the angle between the enemy's and the player's directions
            toPlayerAngle = Vector3.Angle(transform.forward, toPlayerDirection);
            // Gets the distance between the player and the enemy
            rawDistance = Vector3.Distance(playerPosition, transform.position);
            // Gets the distance between the player and the enemy on the Z-Axis
            zDistance = transform.position.z - playerPosition.z;
        }
    }
}
