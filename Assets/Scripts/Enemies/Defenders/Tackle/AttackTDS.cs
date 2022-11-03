using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTDS : TackleState
{
    readonly float animTime = 0.7f;
    readonly float choiceRadius = 2.5f;
    readonly float margin = 1f;

    [Tooltip("- if left / + if right")]
    private float side;
    
    public AttackTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();

        float xPos = enemy.playerPosition.x - enemy.transform.position.x;
        float xVel = enemy.playerVelocity.x;
        if (Mathf.Abs(xPos) <= choiceRadius) side = xVel;
        else side = xPos;

        float dist = Mathf.Min(Mathf.Abs(xPos + xVel * (att.readyDist / enemy.playerVelocity.z)) + margin, att.attackReach);

        if (side != 0) side /= Mathf.Abs(side);

        animator.SetInteger("Side", (int) side);
        animator.SetTrigger(enemy.playerOnGround ? "Attack" : "Jump");

        agent.isStopped = (side == 0);
        agent.updateRotation = (side == 0);
        agent.speed = att.attackSpeed;

        if (side != 0)
        {
            enemy.destination = enemy.transform.position - dist * side * enemy.transform.right;
            agent.velocity = DestinationDir * att.attackSpeed;
        }
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
