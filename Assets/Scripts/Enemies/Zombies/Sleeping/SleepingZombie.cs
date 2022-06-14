using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();

        currentState = new WaitSZS(this, navMeshAgent, animator);
    }

    public override void ChasePlayer()
    {
        base.ChasePlayer();

        currentState = currentState.Process();

        if (playerG.onField && !gameOver)
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    private void Update()
    {
        if (playerG.onField && !gameOver)
        {
            ChasePlayer();
        }
    }
}
