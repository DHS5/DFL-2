using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract class defining the base methods and properties of an enemy
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [Tooltip("Nav Mesh Agent of the enemy")]
    public NavMeshAgent navMeshAgent;

    public AudioSource audioSource;

    [SerializeField] protected Animator animator;

    protected EnemyState currentState;



    [HideInInspector] public Player player;

    [Tooltip("PlayerGameplay component of the player")]
    protected PlayerGameplay playerG;

    [Tooltip("PlayerController component of the player")]
    protected PlayerController playerC;




    [Tooltip("Position of the player")]
    [HideInInspector] public Vector3 playerPosition;
    [Tooltip("Look direction of the player")]
    [HideInInspector] public Vector3 playerLookDirection;
    [Tooltip("Velocity of the player")]
    [HideInInspector] public Vector3 playerForward;
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
    [Tooltip("Whether the player is on the ground or in the air")]
    [HideInInspector] public bool playerOnGround;
    [Tooltip("Whether the player is on the field")]
    [HideInInspector] public bool playerOnField;

    [Tooltip("Destination of the enemy (on the nav mesh)")]
    [HideInInspector] public Vector3 destination;
    

    protected bool gameOver;

    protected bool trucked = false;


    private Vector3 b4StopVelocity;

    //public string state;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
        playerG = player.gameplay;
        playerC = player.controller;

        FindObjectOfType<Renderer>().material = MainManager.InstanceMainManager.FieldManager.stadium.enemyMaterial;
    }



    // ### Functions ###

    public virtual void GetAttribute(EnemyAttributesSO att)
    {
        navMeshAgent.speed = att.speed;
        navMeshAgent.acceleration = att.acceleration;
        navMeshAgent.angularSpeed = att.rotationSpeed;
        navMeshAgent.autoBraking = att.autoBraking;

        Rescale(att);
    }

    private void Rescale(EnemyAttributesSO att)
    {
        var scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x * att.size.x, scale.y * att.size.y, scale.z * att.size.z);
    }

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
            playerLookDirection = player.activeBody.transform.forward.normalized;
            // Gets the player's forward direction
            playerForward = player.transform.forward;
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
            // Find if the player is jumping
            playerOnGround = player.controller.OnGround;
            // Gets whether the player is on the field
            playerOnField = playerG.onField;


            if (audioSource != null) audioSource.enabled = rawDistance < 25;
        }
    }


    public virtual void GameOver()
    {
        gameOver = true;
        animator.SetTrigger("GameOver");
        navMeshAgent.isStopped = true;

        currentState.GameOver();
    }

    public virtual void Trucked(Collision collision)
    {
        if (!trucked)
        {
            trucked = true;
            gameOver = true;
            DestroyColliders();
            Debug.Log("You've been trucked !");
            currentState = currentState.Trucked(collision);
        }
    }

    private void DestroyColliders()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
            Destroy(c);
    }
}
