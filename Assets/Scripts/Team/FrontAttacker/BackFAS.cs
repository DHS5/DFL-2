using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackFAS : FrontAttackerState
{
    public BackFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.BACK;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.PlayerSpeed + att.back2PlayerSpeed;
        agent.avoidancePriority = 99;

        animator.SetTrigger("Sprint");
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + attacker.playerDir * att.positionRadius;

        if (attacker.InZone(attacker.transform.position))
        {
            nextState = new ProtectFAS(attacker, agent, animator);
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
        agent.avoidancePriority = 0;
    }
}
