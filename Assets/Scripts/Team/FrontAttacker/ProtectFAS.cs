using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProtectFAS : AttackerState
{
    new FrontAttacker attacker;

    public ProtectFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.PROTECT;

        attacker = _attacker;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.player.controller.NormalSpeed;

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.transform.position + attacker.playerDir * attacker.positionRadius;
        attacker.destination = attacker.ClampInZone(attacker.destination);

        if (attacker.hasDefender)
        {
            nextState = new DefendFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
        else if (!attacker.InZone(attacker.transform.position))
        {
            nextState = new BackFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
