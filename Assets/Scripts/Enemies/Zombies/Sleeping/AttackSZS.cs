using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackSZS : SleepingZState
{
    public AttackSZS(SleepingZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        enemy = _enemy;
    }

    public override void Enter()
    {        
        base.Enter();

        animator.SetTrigger("Attack");

        Attack();
    }

    public override void Update()
    {
        base.Update();

        if (Time.time >  startTime + 0.2f && agent.remainingDistance < 1f)
        {
            nextState = new FallSZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Attack");
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
            distP = C / -B;
            if (distP < 0 || B == 0)
                distP = att.anticipation;
        }

        else if (delta > 0)
        {
            distP = (-B - Mathf.Sqrt(delta)) / (2 * A);

            if (distP < 0)
            {
                distP = Mathf.Abs(-B + Mathf.Sqrt(delta)) / (2 * A);
            }
        }

        else if (delta == 0)
        {
            //Debug.Log("delta = 0");

            distP = -B / (2 * A);
        }

        else
        {
            //Debug.Log("delta < 0");

            distP = att.anticipation;
        }

        enemy.destination = enemy.playerPosition + distP * enemy.playerVelocity;

        enemy.destination += DestinationDir * att.anticipation;

        agent.velocity = DestinationDir * agent.speed;

        enemy.transform.rotation = Quaternion.LookRotation(DestinationDir);
    }
}
