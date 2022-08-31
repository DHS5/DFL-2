using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTDS : TackleState
{
    readonly float animTime = 0.7f;

    [Tooltip("- if left / + if right")]
    private float side;
    
    public AttackTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();

        side = enemy.playerVelocity.x;
        if (side == 0) side = enemy.playerPosition.x - enemy.transform.position.x;

        animator.SetFloat("Side", side);
        animator.SetTrigger(enemy.playerOnGround ? "Attack" : "Jump");

        agent.isStopped = false;
        agent.updateRotation = false;
        agent.speed = att.attackSpeed;

        enemy.destination = enemy.transform.position - enemy.transform.right * side * att.attackReach;
    }

    public override void Update()
    {
        base.Update();

        if (Time.time - startTime > animTime)
        {
            nextState = new TiredTDS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("Attack");

        base.Exit();
    }
}
