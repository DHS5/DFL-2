using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InterceptIDS : EnemyState
{
    new Interceptor enemy;

    private float interceptTime;

    public InterceptIDS(Interceptor _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.INTERCEPT;

        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();

        Intercept();

        if (!enemy.modeTime && enemy.rawDistance < enemy.attackDist)
        {
            nextState = new AttackIDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
        else if (enemy.modeTime && interceptTime < enemy.attackTime)
        {
            nextState = new AttackIDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }


    private void Intercept()
    {
        float distP;

        float speedC = enemy.playerSpeed / agent.speed;

        float A = 1 - (1 / (speedC * speedC));

        float B = -Mathf.Cos(Vector3.Angle(-enemy.toPlayerDirection, enemy.playerVelocity) * Mathf.Deg2Rad) * 2 * enemy.rawDistance;

        float C = enemy.rawDistance * enemy.rawDistance;

        float delta = (B * B) - (4 * A * C);

        if (A == 0)
        {
            distP = C / -B;
            if (distP < 0)
                distP = enemy.anticipation;
        }

        else if (delta > 0)
        {
            distP = (-B - Mathf.Sqrt(delta)) / (2 * A);

            if (distP < 0)
            {
                distP = (-B + Mathf.Sqrt(delta)) / (2 * A);
            }
        }

        else if (delta == 0)
        {
            Debug.Log("delta = 0");

            distP = -B / (2 * A);
        }

        else
        {
            Debug.Log("delta < 0");

            distP = enemy.anticipation;
        }

        enemy.destination = enemy.playerPosition + enemy.playerVelocity * distP;

        interceptTime = Vector3.Distance(enemy.destination, enemy.transform.position) / agent.speed;
    }
}
