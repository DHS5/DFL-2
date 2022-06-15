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

        animator.SetTrigger("Sprint");

        attacker.UnTarget();
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + attacker.playerDir * attacker.positionRadius;

        if (attacker.InZone(attacker.transform.position))
        {
            nextState = new ProtectFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Sprint");
    }
}
