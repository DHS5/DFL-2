using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseLBDS : LinebackerState
{
    public ChaseLBDS(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;

        enemy.destination = enemy.playerPosition + PlayerDir * (att.anticipation + Mathf.Abs(enemy.xDistance) * att.intelligence);
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");

        enemy.destination = enemy.playerPosition + PlayerDir * (att.anticipation + Mathf.Abs(enemy.xDistance) * att.intelligence);
    }

    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition + PlayerDir * ((enemy.toPlayerAngle < 90 ? att.anticipation : 0) + Mathf.Abs(enemy.xDistance) * att.intelligence);

        // Attack
        if (CanAttack())
        {
            nextState = new AttackLBDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
