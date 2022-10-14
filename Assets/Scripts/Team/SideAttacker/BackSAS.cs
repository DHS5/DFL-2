using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackSAS : SideAttackerState
{
    public BackSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.BACK;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.PlayerSpeed + att.back2PlayerSpeed;

        animator.SetTrigger("Sprint");
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + att.positionRadius * 
            ((int)att.Side * Vector3.Cross(attacker.player.transform.up, attacker.playerDir).normalized + attacker.playerDir).normalized;

        if (attacker.InZone(attacker.transform.position))
        {
            nextState = new ProtectSAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
        else if (attacker.hasDefender)
        {
            nextState = Defend();
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Sprint");
    }
}
