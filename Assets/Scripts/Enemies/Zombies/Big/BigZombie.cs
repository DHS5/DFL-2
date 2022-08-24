using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigZombie : Zombie
{
    public BigZAttributesSO Att { get; private set; }


    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = Attribute as BigZAttributesSO;

        currentState = new WaitBZS(this, navMeshAgent, animator);
    }

    public override void ChasePlayer()
    {
        base.ChasePlayer();

        currentState = currentState.Process();

        if (playerG.onField && !dead)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (Attribute.reactivity != 0 && !gameOver && !dead)
        {
            Invoke(nameof(ChasePlayer), Attribute.reactivity);
        }
    }

    private void Update()
    {
        if (Attribute.reactivity == 0 && playerG.onField && !gameOver && !dead)
        {
            ChasePlayer();
        }
    }
}
