using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitLDS : EnemyState
{
    public WaitLDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;
    }


    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.transform.position;

        enemy.transform.LookAt(enemy.player.transform);

        
        if (enemy.zDistance < enemy.waitDist)
        {
            // Positionning
            nextState = new PositionningLDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }
}
