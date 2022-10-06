using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackLBDS : LinebackerState
{
    readonly float animTime = 1.6f;
    
    public AttackLBDS(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Attack");

        agent.speed = att.attackSpeed;

        Attack();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time - startTime > animTime / 2 && Time.time - startTime < 3 * animTime / 4)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        if (Time.time - startTime > 3 * animTime / 4)
        {
            agent.isStopped = false;
            enemy.destination = enemy.playerPosition;
            enemy.destination += DestinationDir * 3;
            agent.speed = att.speed;
        }

        if (Time.time - startTime > animTime)
        {
            nextState = new ChaseLBDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        agent.speed = att.speed;

        animator.ResetTrigger("Attack");

        base.Exit();
    }



    private void Attack()
    {
        float distP;

        float speedC = (enemy.playerSpeed != 0 ? enemy.playerSpeed : 0.1f) / agent.speed;

        float A = 1 - (1 / (speedC * speedC));

        float B = -Mathf.Cos(Vector3.Angle(-enemy.toPlayerDirection, enemy.playerVelocity) * Mathf.Deg2Rad) * 2 * enemy.rawDistance;

        float C = enemy.rawDistance * enemy.rawDistance;

        float delta = (B * B) - (4 * A * C);

        if (A == 0)
        {
            Debug.Log("A == 0");
            distP = C / -B;
            if (distP < 0 || B == 0)
                distP = att.anticipation;
        }

        else if (delta > 0)
        {
            Debug.Log("Delta > 0");
            distP = (-B - Mathf.Sqrt(delta)) / (2 * A);

            if (distP < 0)
            {
                distP = Mathf.Abs(-B + Mathf.Sqrt(delta)) / (2 * A);
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

            distP = att.anticipation;
        }

        enemy.destination = enemy.playerPosition + distP * att.attackPrecision * enemy.playerVelocity;

        if (ToDestinationAngle > att.attackAngle)
        {
            enemy.destination = enemy.playerPosition;
            Debug.Log("straight to it");
        }

        enemy.destination += DestinationDir * 5;

        agent.velocity = DestinationDir * att.attackSpeed;

        enemy.transform.rotation = Quaternion.LookRotation(DestinationDir);
    }
}
