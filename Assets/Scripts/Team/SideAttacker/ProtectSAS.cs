using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProtectSAS : AttackerState
{
    new SideAttacker attacker;

    private int side;

    public ProtectSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator, int _side) : base(_attacker, _agent, _animator)
    {
        name = AState.PROTECT;

        attacker = _attacker;

        side = _side;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.player.controller.playerAtt.NormalSpeed;

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.transform.position + attacker.playerDir * attacker.positionRadius;
        attacker.destination = attacker.ClampInZone(attacker.destination);

        if (attacker.hasDefender)
        {
            nextState = new DefendSAS(attacker, agent, animator, side);
            stage = Event.EXIT;
        }
        else if (!attacker.InZone(attacker.transform.position))
        {
            nextState = new BackSAS(attacker, agent, animator, side);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
