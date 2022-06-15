using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackFAS : AttackerState
{
    new FrontAttacker attacker;

    public BackFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.BACK;

        attacker = _attacker;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.back2PlayerSpeed;

        attacker.UnTarget();
    }
}
