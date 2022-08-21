using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackSDS : SafetyState
{
    private float baseSpeed;
    
    public AttackSDS(Safety _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        baseSpeed = att.speed;

        agent.speed = att.attackSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Chase");
    }


    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.playerPosition;

        if (enemy.rawDistance > att.attackDist)
        {
            nextState = new ChaseSDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        agent.speed = baseSpeed;

        animator.ResetTrigger("Chase");

        base.Exit();
    }
}