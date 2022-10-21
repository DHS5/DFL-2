using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningBZS : BigZState
{
    public PositionningBZS(BigZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.POSITIONNING;

        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Walk");
    }

    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition + Mathf.Min(1, Mathf.Abs(enemy.xDistance) / att.waitXRange) * enemy.zDistance * enemy.playerForward;


        if (enemy.rawDistance < att.attackDist)
        {
            nextState = new AttackBZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Walk");
    }
}
