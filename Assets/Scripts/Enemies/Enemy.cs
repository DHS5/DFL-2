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
    [Tooltip("Game Object of the player")]
    protected GameObject player;

    [HideInInspector] public AudioSource audioSource;

    protected Animator animator;

    [Header("Behaviour parameters")]
    [Tooltip("Level of intelligence of the enemy (anticipation of the future position)")]
    [SerializeField] protected int intelligence;
    [Tooltip("Level of reactivity of the enemy (time between new destination's settings)")]
    [SerializeField] protected float reactivity;
    [Tooltip("Reactivity multiplier")] 
    protected float reactivityM;

    [Tooltip("Radius around the enemy in which the player gets the enemy's attention")]
    [SerializeField] protected float attentionRadius;
    [Tooltip("Radius around the enemy in which the player activates the enemy's chase")]
    [SerializeField] protected float chaseRadius;
    [Tooltip("Radius around the enemy in which the player activates the enemy's attack")]
    [SerializeField] protected float attackRadius;


    [Header("Physical parameters")]
    [Tooltip("Attack speed multiplier")]
    [SerializeField] protected float attackSpeedM;
    [Tooltip("Whether the enemy is in attack speed")]
    protected bool attackSpeed = false;

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
    protected Vector3 playerPosition;
    [Tooltip("Direction of the player")]
    protected Vector3 playerDirection;
    [Tooltip("Direction from the enemy to the player")]
    protected Vector3 toPlayerDirection;
    [Tooltip("Distance between the enemy and the player")]
    protected float distance;
    [Tooltip("Angle between the enemy and the player")]
    protected float toPlayerAngle;
    [Tooltip("Destination of the enemy (on the nav mesh)")]
    protected Vector3 destination;

    /// <summary>
    /// Stops the enemy
    /// </summary>
    public void Stop()
    {
        navMeshAgent.isStopped = true;
    }
    /// <summary>
    /// Resumes the enemy
    /// </summary>
    public void Resume()
    {
        navMeshAgent.isStopped = false;
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
            // Gets the direction to the player
            toPlayerDirection = (playerPosition - transform.position).normalized;
            // Gets the distance between the player and the enemy
            distance = Vector3.Distance(playerPosition, transform.position);
            // Gets the angle between the enemy's and the player's directions
            toPlayerAngle = Vector3.Angle(transform.forward, toPlayerDirection);
        }
    }

    /// <summary>
    /// Method to get the attack speed back to normal
    /// </summary>
    protected void NormalSpeed() { navMeshAgent.speed /= attackSpeedM; attackSpeed = false; }

    /// <summary>
    /// Gets the player Game Object and the NavMesh Agent
    /// </summary>
    protected void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

}
