using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InterceptWDS : EnemyState
{
    public InterceptWDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.INTERCEPT;
    }


    public override void Update()
    {
        base.Update();


        enemy.destination = enemy.playerPosition + enemy.playerDirection * ( enemy.anticipation +
             ( (enemy.rawDistance / agent.speed) // Minimum time to reach the destination
             * enemy.playerSpeed) // * playerSpeed to have the distance the player will have run
             * enemy.intelligence); // * intelligence (0 ... 1)

        if (enemy.rawDistance < enemy.chaseDist)
        {
            nextState = new ChaseWDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }
}
