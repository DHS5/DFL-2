using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackLDS : LineManState
{
    readonly float animTime = 1.6f;

    // Compensation for : * enemy.attackSpeed
    readonly float attackOffset = 0.2f;
    
    public AttackLDS(LineMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Attack");

        agent.speed = att.attackSpeed;

        enemy.destination = enemy.playerPosition + enemy.playerVelocity * att.attackAnticipation;
        
        enemy.destination += att.attackSpeed * attackOffset * DestinationDir;
        
        agent.velocity = DestinationDir * att.attackSpeed;

        enemy.transform.rotation = Quaternion.LookRotation(DestinationDir);
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
        agent.speed = att.speed;

        animator.ResetTrigger("Attack");

        base.Exit();
    }
}
