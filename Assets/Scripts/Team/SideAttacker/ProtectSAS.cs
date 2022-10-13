using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProtectSAS : SideAttackerState
{
    public ProtectSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.PROTECT;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.PlayerSpeed;

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.transform.position + attacker.playerDir * att.positionRadius;
        attacker.destination = attacker.ClampInZone(attacker.destination);

        if (attacker.hasDefender)
        {
            nextState = Defend();
            stage = Event.EXIT;
        }
        else if (!attacker.InZone(attacker.transform.position))
        {
            nextState = new BackSAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
