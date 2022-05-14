using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private void Start()
    {
        animator.Play("Base Layer.Walk", 0, Random.Range(0f, 1f));
    }

    private void Update()
    {    
        // Run
        if (Mathf.Abs(navMeshAgent.velocity.magnitude) > 0.01f)
        {
            animator.SetFloat("MoveSpeed", -1f);
            animator.SetFloat("SpeedM", navMeshAgent.velocity.magnitude);
        }
        // Idle
        else
        {
            animator.SetFloat("MoveSpeed", 0f);
            animator.SetFloat("SpeedM", 3f);
        }
        // Attack
        if (distance < attackRadius && distance > 0 && navMeshAgent.velocity.magnitude > 0)
        {
            animator.SetTrigger("Attack");
        }
    }
}
