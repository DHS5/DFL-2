using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitSDS : SafetyState
{
    public WaitSDS(Safety _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
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
            if (enemy.zDistance < att.waitDist)
            {
                // If out the precision cone
                if (Mathf.Abs(Vector3.Angle(PlayerDir, -enemy.toPlayerDirection)) > att.waitAngle + att.waitMargin || Mathf.Abs(enemy.toPlayerAngle) > att.backAngle)
                {
                    // Intercept
                    if (enemy.rawDistance > att.chaseDist)
                        nextState = new InterceptSDS(enemy, agent, animator);
                    // Chase
                    else
                        nextState = new ChaseSDS(enemy, agent, animator);

                    stage = Event.EXIT;
                }
                // If in the precision cone and distance < patience --> attack
                else if (enemy.rawDistance < att.patience)
                {
                    nextState = new ChaseSDS(enemy, agent, animator);
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
