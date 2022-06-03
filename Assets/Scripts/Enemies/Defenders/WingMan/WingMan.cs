using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingMan : Defender
{
    protected override void Start()
    {
        base.Start();

        currentState = new WaitWDS(this, navMeshAgent, animator);
    }

    /// <summary>
    /// Gives the NavMeshAgent his destination
    /// </summary>
    public override void ChasePlayer()
    {
        base.ChasePlayer();

        currentState = currentState.Process();

        if (reactivity != 0)
        {
            Invoke(nameof(ChasePlayer), reactivity);
        }
    }

    private void Update()
    {
        if (reactivity == 0)
        {
            ChasePlayer();
        }
    }
}
