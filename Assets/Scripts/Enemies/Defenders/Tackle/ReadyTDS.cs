using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReadyTDS : TackleState
{
    public ReadyTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.READY;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Ready");

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        enemy.transform.LookAt(enemy.player.transform);
    }


    public override void Update()
    {
        base.Update();


        if (enemy.zDistance < att.readyDist)
        {
            if (enemy.rawDistance < att.attackDist)
                nextState = new AttackTDS(enemy, agent, animator);

            else
                nextState = new TiredTDS(enemy, agent, animator);

            stage = Event.EXIT;
        }
    }


    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Ready");
    }
}
