using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();

        currentState = new WaitCZS(this, navMeshAgent, animator);
    }

    public override void ChasePlayer()
    {
        base.ChasePlayer();

        currentState = currentState.Process();

        if (playerG.onField && !dead)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (reactivity != 0 && !gameOver && !dead)
        {
            Invoke(nameof(ChasePlayer), reactivity);
        }
    }

    private void Update()
    {
        if (reactivity == 0 && playerG.onField && !gameOver && !dead)
        {
            ChasePlayer();
        }
    }
}
