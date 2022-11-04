using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningLDS : LineManState
{
    private float preciseXDistance;
    private bool arriveInPrecision = true;

    public PositionningLDS(LineMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.POSITIONNING;
    }


    public override void Update()
    {
        base.Update();

        
        preciseXDistance = Mathf.Abs(enemy.transform.position.x - (enemy.playerPosition + att.positioningRatio * enemy.zDistance * PlayerDir).x);
        
        if (preciseXDistance > enemy.precision)
        {
            arriveInPrecision = true;
            agent.speed = att.speed;
            agent.angularSpeed = att.rotationSpeed;
            animator.SetTrigger("Run");
            animator.ResetTrigger("Wait");
            animator.ResetTrigger("Walk");
            agent.isStopped = false;
            enemy.destination = enemy.playerPosition + (Mathf.Clamp(enemy.zDistance / att.waitDist, 0, Mathf.Max(0, 1 - att.positioningRatio)) + Mathf.Max(Mathf.Abs(enemy.xDistance / enemy.zDistance), att.positioningRatio)) * enemy.zDistance * PlayerDir;
        }
        else if (preciseXDistance <= enemy.precision && enemy.toPlayerAngle > 5)
        {
            if (arriveInPrecision)
            {
                agent.velocity = agent.velocity.normalized * enemy.precision;
                arriveInPrecision = false;
                animator.ResetTrigger("Run");
                animator.ResetTrigger("Wait");
                animator.SetTrigger("Walk");
            }
            agent.isStopped = false;
            agent.speed = enemy.precision;
            agent.angularSpeed = att.rotationSpeed * 20;
            enemy.destination = enemy.playerPosition;
        }
        else
        {
            arriveInPrecision = true;
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Wait");
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (enemy.zDistance < att.positionningDist)
        {
            // Attack
            if (enemy.rawDistance < att.attackDist && enemy.toPlayerAngle < att.attackAngle)
                nextState = new AttackLDS(enemy, agent, animator);
            // Chase
            else
                nextState = new ChaseLDS(enemy, agent, animator);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
        animator.ResetTrigger("Wait");
        animator.ResetTrigger("Walk");
        agent.isStopped = false;
    }
}
