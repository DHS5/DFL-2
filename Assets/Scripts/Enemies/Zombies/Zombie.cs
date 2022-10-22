using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public ZombieAttributesSO Attribute { get; private set; }

    [HideInInspector] public bool dead;

    private void Start()
    {
        dead = false;
    }

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        if (att != null)
            Attribute = att as ZombieAttributesSO;
    }


    public override void ChasePlayer()
    {
        base.ChasePlayer();
        
        currentState = currentState.Process();
        if (playerG.onField && !gameOver && !dead)
        {
            navMeshAgent.SetDestination(destination);
        }
    }



    public virtual void Dead()
    {
       currentState.GameOver();

        dead = true;
        navMeshAgent.isStopped = true;

        animator.SetBool("Dead", true);
        animator.SetTrigger("Die");
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }
}
