using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningLDS : EnemyState
{
    public PositionningLDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.POSITIONNING;
    }


    public override void Update()
    {
        base.Update();


        if (enemy.xDistance > enemy.precision)
            enemy.destination = enemy.playerPosition + enemy.intelligence * enemy.zDistance * enemy.playerLookDirection;
        else
        {
            enemy.destination = enemy.transform.position;
            enemy.transform.localRotation = 
                Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(enemy.playerPosition - enemy.transform.position), 5 * Time.deltaTime);
        }

        if (enemy.zDistance < enemy.positionningDist)
        {
            nextState = new AttackLDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }
}
