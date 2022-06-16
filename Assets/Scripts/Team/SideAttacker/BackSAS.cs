using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackSAS : AttackerState
{
    new SideAttacker attacker;

    private int side;

    public BackSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator, int _side) : base(_attacker, _agent, _animator)
    {
        name = AState.BACK;

        attacker = _attacker;

        side = _side;
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

        attacker.destination = attacker.playerPos + attacker.positionRadius * 
            (side * Vector3.Cross(attacker.player.transform.up, attacker.playerDir).normalized + attacker.playerDir).normalized;

        if (attacker.InZone(attacker.transform.position))
        {
            nextState = new ProtectSAS(attacker, agent, animator, side);
            stage = Event.EXIT;
        }
        else if (attacker.hasDefender)
        {
            nextState = new DefendSAS(attacker, agent, animator, side);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Sprint");
    }
}
