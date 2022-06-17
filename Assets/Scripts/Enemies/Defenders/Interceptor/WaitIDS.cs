using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitIDS : EnemyState
{
    new Interceptor enemy;

    public WaitIDS(Interceptor _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;

        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Wait");
    }

    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.transform.position;

        if (enemy.playerOnField)
        {
            if (enemy.zDistance < enemy.waitDist)
            {
                nextState = new InterceptIDS(enemy, agent, animator);
                stage = Event.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Wait");
    }
}
