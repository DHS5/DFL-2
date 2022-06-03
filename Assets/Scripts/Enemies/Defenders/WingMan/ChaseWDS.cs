using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseWDS : EnemyState
{
    public ChaseWDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;
    }


    public override void Update()
    {
        base.Update();


        enemy.destination = enemy.playerPosition + enemy.playerDirection * enemy.anticipation;

        // Attack
        if (enemy.rawDistance < enemy.attackDist)
        {
            nextState = new AttackWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
        // Intercept
        if (enemy.rawDistance > enemy.chaseDist)
        {
            nextState = new InterceptWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }
}
