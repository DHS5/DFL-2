using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitBZS : BigZState
{
    public WaitBZS(BigZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;

        enemy = _enemy;
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


        if (enemy.playerOnField && enemy.zDistance < att.waitDist && enemy.zDistance > 0 && enemy.xDistance > att.waitXRange)
        {
            nextState = new PositionningBZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
        else if (enemy.rawDistance < att.attackDist)
        {
            nextState = new AttackBZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Wait");
    }
}
