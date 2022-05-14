using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attackers : MonoBehaviour
{
    [Tooltip("")]
    [HideInInspector] public TeamManager teamManager;


    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject target;


    [HideInInspector] public float playerProtectionRadius;
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


    private float runLimit = 3f;
    private float walkLimit = 0.0001f;

    private Quaternion lookRot;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        // Walk
        if (navMeshAgent.velocity.magnitude > walkLimit && navMeshAgent.velocity.magnitude < runLimit)
        {
            animator.SetFloat("Speed", 1f);
            if (target != null) lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
            else lookRot = transform.rotation;
            navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRot, 5 * Time.deltaTime);
        }
        // Run
        else if (navMeshAgent.velocity.magnitude >= runLimit)
        {
            animator.SetFloat("Speed", 2f);
        }
        // Idle
        else
        {
            animator.SetFloat("Speed", 0f);

            if (target != null) lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
            else lookRot = transform.rotation;
            navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRot, 5 * Time.deltaTime);
        }
    }



    /// <summary>
    /// Stops the attacker
    /// </summary>
    public void Stop()
    {
        navMeshAgent.isStopped = true;
    }
    /// <summary>
    /// Resumes the attacker
    /// </summary>
    public void Resume()
    {
        navMeshAgent.isStopped = false;
    }


    public virtual void TargetEnemy(GameObject enemy)
    {
        if (hasDefender)
        {
            teamManager.AddEnemy(target);
        }
        teamManager.SuppEnemy(enemy);
        hasDefender = true;
        target = enemy;
        BlockEnemy();
    }

    protected virtual void UnTarget()
    {
        hasDefender = false;
        teamManager.AddEnemy(target);
        teamManager.FreeAttacker(gameObject);
    }

    protected virtual void BlockEnemy()
    {

    }
}
