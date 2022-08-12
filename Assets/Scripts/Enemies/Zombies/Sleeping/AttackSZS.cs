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

        float distP;

        float speedC = enemy.playerSpeed / agent.speed;

        float A = 1 - (1 / (speedC * speedC));

        float B = - Mathf.Cos(Vector3.Angle(-enemy.toPlayerDirection, enemy.playerVelocity) * Mathf.Deg2Rad) * 2 * enemy.rawDistance;

        float C = enemy.rawDistance * enemy.rawDistance;

        float delta = (B * B) - (4 * A * C);

        if (A == 0)
        {
            distP = C / -B;
            if (distP < 0)
                distP = att.anticipation;
        }

        else if (delta > 0)
        {
            distP = (-B - Mathf.Sqrt(delta)) / (2 * A);

            if (distP < 0)
            {
                Debug.Log("dist < 0");
                
                distP = (-B + Mathf.Sqrt(delta)) / (2 * A);
            }
        }

        else if (delta == 0)
        {
            Debug.Log("Delta = 0");

            distP = -B / (2 * A);
        }

        else
        {
            Debug.Log("Delta < 0");

            distP = 0;
        }

        enemy.destination = enemy.playerPosition + enemy.playerVelocity * distP;
        Vector3 destinationDir = (enemy.destination - enemy.transform.position).normalized;
        enemy.destination += destinationDir * att.anticipation;

        agent.velocity = destinationDir * agent.speed;
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
}
