using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackBAS : AttackerState
{
    new BackAttacker attacker;

    public BackBAS(BackAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.BACK;

        attacker = _attacker;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.back2PlayerSpeed;

        animator.SetTrigger("Sprint");
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos - attacker.playerDir * (attacker.positionRadius / 2);

        if (attacker.InZone(attacker.transform.position))
        {
            nextState = new ProtectBAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
        else if (attacker.hasDefender)
        {
            nextState = new DefendBAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Sprint");
    }
}
