using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitWDS : EnemyState
{
    public WaitWDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;
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
            if (!enemy.patient && enemy.zDistance < enemy.waitDist)
            {
                // Intercept
                if (enemy.rawDistance > enemy.chaseDist)
                    nextState = new InterceptWDS(enemy, agent, animator);
                // Chase
                else
                    nextState = new ChaseWDS(enemy, agent, animator);

                stage = Event.EXIT;
            }

            if (enemy.patient && enemy.zDistance < enemy.waitDist)
            {
                // If out the precision cone
                if (Mathf.Abs(Vector3.Angle(enemy.playerVelocity, -enemy.toPlayerDirection)) > enemy.precision)
                {
                    // Intercept
                    if (enemy.rawDistance > enemy.chaseDist)
                        nextState = new InterceptWDS(enemy, agent, animator);
                    // Chase
                    else
                        nextState = new ChaseWDS(enemy, agent, animator);

                    stage = Event.EXIT;
                }
                // If in the precision cone and distance < patience --> attack
                else if (enemy.rawDistance < enemy.patience)
                {
                    nextState = new AttackWDS(enemy, agent, animator);
                    stage = Event.EXIT;
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Wait");
    }
}
