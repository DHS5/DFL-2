using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingZombie : Zombie
{
    public SleepingZAttributesSO Att { get; private set; }


    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = Attribute as SleepingZAttributesSO;

        currentState = new WaitSZS(this, navMeshAgent, animator);
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

    private void Update()
    {
        if (playerG.onField && !gameOver && !dead)
        {
            ChasePlayer();
        }
    }
}
