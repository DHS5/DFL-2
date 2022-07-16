using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackIDS : EnemyState
{
    new Interceptor enemy;

    private float baseSpeed;

    readonly private float animTime = 1.6f;
    
    public AttackIDS(Interceptor _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Attack");

        baseSpeed = agent.speed;
        agent.speed = enemy.attackSpeed;

        Attack();
    }


    public override void Update()
    {
        base.Update();

        if (Time.time - startTime > animTime / 2 && Time.time - startTime < 3 * animTime / 4)
        {
            enemy.destination = enemy.transform.position;
        }
        if (Time.time - startTime > 3 * animTime / 4)
        {
            enemy.destination = enemy.playerPosition;
        }

        if (Time.time - startTime > animTime)
        {
            if (enemy.rawDistance > enemy.attackDist)
                nextState = new InterceptIDS(enemy, agent, animator);
            else 
                nextState = new AttackIDS(enemy, agent, animator);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        agent.speed = baseSpeed;

        animator.ResetTrigger("Attack");

        base.Exit();
    }


    private void Attack()
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

        agent.velocity = (enemy.destination - enemy.transform.position).normalized * enemy.attackSpeed;
    }
}
