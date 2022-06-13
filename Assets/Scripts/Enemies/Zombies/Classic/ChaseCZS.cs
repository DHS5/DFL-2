using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseCZS : EnemyState
{
    new ClassicZombie enemy;
    public ChaseCZS(ClassicZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.CHASE;

        enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition;


        if (enemy.rawDistance < enemy.attackDist)
        {
            nextState = new AttackCZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }
}
