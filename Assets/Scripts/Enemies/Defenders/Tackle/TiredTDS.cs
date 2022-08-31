using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TiredTDS : TackleState
{
    public TiredTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.TIRED;

        agent.isStopped = true;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Tired");
    }
}
