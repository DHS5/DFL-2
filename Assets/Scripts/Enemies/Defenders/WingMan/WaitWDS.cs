using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitWDS : EnemyState
{
    public WaitWDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;
    }


    public override void Update()
    {
        base.Update();

        
        if (enemy.zDistance < enemy.waitDist)
        {
            // Intercept
            if (enemy.rawDistance > enemy.chaseDist)
                nextState = new InterceptWDS(enemy, agent, animator);
            // Chase
            else
                nextState = new ChaseWDS(enemy, agent, animator);

            stage = Event.EXIT;
        }
    }
}
