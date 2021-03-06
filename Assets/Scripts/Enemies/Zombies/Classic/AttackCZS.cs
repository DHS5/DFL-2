using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackCZS : EnemyState
{
    new ClassicZombie enemy;
    
    private float baseSpeed;

    public AttackCZS(ClassicZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        baseSpeed = agent.speed;

        agent.speed = enemy.attackSpeed;

        animator.SetTrigger("Attack");
    }

    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition;


        if (enemy.rawDistance > enemy.attackDist)
        {
            nextState = new ChaseCZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        agent.speed = baseSpeed;

        animator.ResetTrigger("Attack");
    }
}
