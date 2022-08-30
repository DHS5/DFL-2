using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTDS : TackleState
{
    readonly float animTime = 1.6f;

    [Tooltip("- if left / + if right")]
    private float side;
    
    public AttackTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;

        side = enemy.playerPosition.x - enemy.transform.position.x;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetFloat("Side", side);
        animator.SetTrigger(enemy.playerOnGround ? "Attack" : "Jump");
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
