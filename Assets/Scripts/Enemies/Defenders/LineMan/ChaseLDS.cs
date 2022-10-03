using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseLDS : LineManState
{
    public ChaseLDS(LineMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;
    }


    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Run");
    }

    public override void Update()
    {
        base.Update();


        enemy.destination = enemy.playerPosition + PlayerDir * (att.anticipation + Mathf.Abs(enemy.xDistance / enemy.zDistance) * att.intelligence);

        // Attack
        if (enemy.rawDistance < att.attackDist && enemy.toPlayerAngle < att.attackAngle)
        {
            nextState = new AttackLDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
    }
}
