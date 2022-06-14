using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingMan : Defender
{
    protected override void Awake()
    {
        base.Awake();

        currentState = new WaitWDS(this, navMeshAgent, animator);
    }

    /// <summary>
    /// Gives the NavMeshAgent his destination
    /// </summary>
    public override void ChasePlayer()
    {
        base.ChasePlayer();

        currentState = currentState.Process();

        if (playerG.onField)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (reactivity != 0 && !gameOver)
        {
            Invoke(nameof(ChasePlayer), reactivity);
        }
    }

    private void Update()
    {
        if (reactivity == 0 && playerG.onField && !gameOver)
        {
            ChasePlayer();
        }

    }
}
