using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InterceptWDS : WingManState
{
    public InterceptWDS(WingMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.INTERCEPT;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();

        if (Mathf.Abs(enemy.toPlayerAngle) < 90 || enemy.zDistance < 0)
        {
            enemy.destination = enemy.playerPosition + enemy.playerVelocity * (att.anticipation +
                 ((enemy.rawDistance / enemy.Attribute.speed) // Minimum time to reach the destination
                 * enemy.playerSpeed) // * playerSpeed to have the distance the player will have run
                 * att.intelligence); // * intelligence (0 ... 1)
        }
        else
        {
            float dist = enemy.rawDistance / Mathf.Cos( Mathf.Deg2Rad * Mathf.Abs(Vector3.Angle(-enemy.toPlayerDirection, enemy.playerVelocity)) );
            enemy.destination = enemy.playerPosition + enemy.playerVelocity * dist;
        }

        if (enemy.rawDistance < att.chaseDist)
        {
            nextState = new ChaseWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
        if (Mathf.Abs(Vector3.Angle(enemy.playerVelocity, -enemy.toPlayerDirection)) < att.chaseAngle)
        {
            // Wait
            if (att.patient)
            {
                nextState = new WaitWDS(enemy, agent, animator);
            }
            // Chase
            else
            {
                nextState = new ChaseWDS(enemy, agent, animator);
            }
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
