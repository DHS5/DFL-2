using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningLBDS : LinebackerState
{
    private bool arriveInPrecision = true;
    public PositionningLBDS(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.POSITIONNING;
    }

    public override void Update()
    {
        base.Update();


        if (enemy.xDistance > enemy.precision)
        {
            arriveInPrecision = true;
            agent.speed = att.speed;
            agent.angularSpeed = att.rotationSpeed;
            animator.SetTrigger("Run");
            animator.ResetTrigger("Wait");
            animator.ResetTrigger("Walk");
            agent.isStopped = false;
            enemy.destination = enemy.playerPosition + Mathf.Max(Mathf.Abs(enemy.xDistance / enemy.zDistance), 1) * enemy.zDistance * enemy.playerForward;
        }
        else if (enemy.xDistance <= enemy.precision && enemy.toPlayerAngle > 5)
        {
            if (arriveInPrecision)
            {
                //agent.velocity = agent.velocity.normalized * enemy.precision;
                agent.velocity = Vector3.zero;
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
            if (CanAttack())
                nextState = new AttackLBDS(enemy, agent, animator);
            // Chase
            else
                nextState = new ChaseLBDS(enemy, agent, animator);

            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Run");
        animator.ResetTrigger("Wait");

        agent.updateRotation = true;
    }
}
