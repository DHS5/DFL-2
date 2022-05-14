using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Enemy
{
    private float runLimit = 3f;
    private float walkLimit = 0.0001f;

    private Quaternion lookRot;
    
    public override void ChasePlayer()
    {
        base.ChasePlayer();
    }

    private void Start()
    {
        animator.Play("Base Layer.Idle", 0, Random.Range(0f, 1f));
        gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Update()
    {
        // Walk
        if (navMeshAgent.velocity.magnitude > walkLimit && navMeshAgent.velocity.magnitude < runLimit)
        {
            animator.SetFloat("Speed", 1f);
            lookRot = Quaternion.LookRotation(playerPosition - transform.position);
            navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRot, 5 * Time.deltaTime);
            audioSource.Stop();
        }
        // Run
        else if (navMeshAgent.velocity.magnitude >= runLimit)
        {
            animator.SetFloat("Speed", 2f);
            audioSource.priority = (int)distance;
            if (!audioSource.isPlaying) audioSource.Play();
        }
        // Idle
        else
        {
            animator.SetFloat("Speed", 0f);
            lookRot = Quaternion.LookRotation(playerPosition - transform.position);
            navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, lookRot, 5 * Time.deltaTime);
            audioSource.Stop();
        }

        // Attack
        if (distance < 5 && distance > 0 && navMeshAgent.velocity.magnitude > walkLimit)
        {
            animator.SetFloat("Speed", 3f);
        }
    }
}
