using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseWDS : EnemyState
{
    public ChaseWDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");
    }


    public override void Update()
    {
        base.Update();


        enemy.destination = enemy.playerPosition + enemy.playerVelocity * enemy.anticipation;

        // Attack
        if (enemy.rawDistance < enemy.attackDist)
        {
            nextState = new AttackWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
        // Intercept
        if (enemy.rawDistance > enemy.chaseDist && Mathf.Abs(Vector3.Angle(enemy.playerVelocity, -enemy.toPlayerDirection)) > enemy.precision)
        {
            nextState = new InterceptWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
