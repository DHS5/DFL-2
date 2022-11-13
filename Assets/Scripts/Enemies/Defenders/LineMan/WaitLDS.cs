using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitLDS : LineManState
{
    public WaitLDS(LineMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
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


        if (enemy.playerOnField && enemy.zDistance < att.waitDist)
        {
            // Positionning
            nextState = new PositionningLDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Wait");
    }
}
