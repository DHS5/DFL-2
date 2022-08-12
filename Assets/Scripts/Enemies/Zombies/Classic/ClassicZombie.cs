using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicZombie : Zombie
{
    public ClassicZAttributesSO Att { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Att = Attribute as ClassicZAttributesSO;

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
