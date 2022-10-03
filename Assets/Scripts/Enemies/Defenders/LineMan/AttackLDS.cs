using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackLDS : LineManState
{
    readonly float animTime = 1.6f;

    // Compensation for : * enemy.attackSpeed
    readonly float attackOffset = 0.2f;
    
    private float baseSpeed;
    
    public AttackLDS(LineMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        baseSpeed = agent.speed;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Attack");

        agent.speed = att.attackSpeed;

        Vector3 playerDir = (enemy.playerPosition - enemy.transform.position).normalized;
        
        enemy.destination = enemy.playerPosition + att.attackSpeed * attackOffset * playerDir;
        
        agent.velocity = playerDir * att.attackSpeed;

        enemy.transform.LookAt(enemy.player.activeBody.transform);
    }

    public override void Update()
    {
        base.Update();

        if (Time.time - startTime > animTime / 2 && Time.time - startTime < 3 * animTime / 4)
        {
            enemy.destination = enemy.transform.position;
        }
        if (Time.time - startTime > 3 * animTime / 4)
        {
            enemy.destination = enemy.playerPosition;
        }

        if (Time.time - startTime > animTime)
        {
            nextState = new ChaseLDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        agent.speed = baseSpeed;

        animator.ResetTrigger("Attack");

        base.Exit();
    }
}
