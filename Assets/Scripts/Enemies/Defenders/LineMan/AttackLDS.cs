using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackLDS : EnemyState
{
    private float baseSpeed;
    
    public AttackLDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        baseSpeed = agent.speed;
    }


    public override void Update()
    {
        base.Update();

        agent.speed = enemy.attackSpeed;

        enemy.destination = enemy.playerPosition;

        if (enemy.rawDistance > enemy.attackDist)
        {
            nextState = new ChaseLDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        agent.speed = baseSpeed;

        base.Exit();
    }
}
