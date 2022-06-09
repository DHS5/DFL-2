using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMan : Defender
{
    protected override void Awake()
    {
        base.Awake();

        currentState = new WaitLDS(this, navMeshAgent, animator);

        precision = Random.Range(0, precision * intelligence);
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

        if (reactivity != 0)
        {
            Invoke(nameof(ChasePlayer), reactivity);
        }
    }

    private void Update()
    {
        if (reactivity == 0 && playerG.onField)
        {
            ChasePlayer();
        }

    }
}
