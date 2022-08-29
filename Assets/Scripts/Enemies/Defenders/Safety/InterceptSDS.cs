using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InterceptSDS : SafetyState
{
    public InterceptSDS(Safety _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
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
            enemy.destination = enemy.playerPosition + PlayerDir * (att.anticipation +
                 ((enemy.rawDistance / att.speed) // Minimum time to reach the destination
                 * enemy.playerSpeed) // * playerSpeed to have the distance the player will have run
                 * att.intelligence); // * intelligence (0 ... 1)
        }
        else
        {
            float dist = enemy.rawDistance / Mathf.Cos( Mathf.Deg2Rad * Mathf.Abs(Vector3.Angle(-enemy.toPlayerDirection, PlayerDir)) );
            enemy.destination = enemy.playerPosition + PlayerDir * dist;
        }

        if (enemy.rawDistance < att.chaseDist)
        {
            nextState = new ChaseSDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
        if (Mathf.Abs(Vector3.Angle(PlayerDir, -enemy.toPlayerDirection)) < att.waitAngle)
        {
            nextState = new WaitSDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
