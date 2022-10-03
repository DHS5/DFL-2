using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackWDS : WingManState
{
    public AttackWDS(WingMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        agent.speed = att.attackSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Chase");
    }


    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition;
        enemy.destination += DestinationDir * 3;

        if (enemy.rawDistance > att.attackDist || enemy.toPlayerAngle > att.attackAngle)
        {
            nextState = new ChaseWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        agent.speed = att.speed;

        animator.ResetTrigger("Chase");

        base.Exit();
    }
}
