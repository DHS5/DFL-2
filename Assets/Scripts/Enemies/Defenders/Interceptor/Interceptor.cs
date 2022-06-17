using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Defender
{
    [Tooltip("Mode of the interceptor :\n" +
        "TRUE = time --> base his attack on Attack time\n" +
        "FALSE = distance --> base his attack on Attack dist")]
    public bool modeTime;

    [Tooltip("Time before impact at which the interceptor attacks")]
    public float attackTime;

    protected override void Awake()
    {
        base.Awake();

        currentState = new WaitIDS(this, navMeshAgent, animator);
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
