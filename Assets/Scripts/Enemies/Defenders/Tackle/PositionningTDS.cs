using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningTDS : TackleState
{
    private float anticipation { get { return att.anticipation * enemy.playerSpeed * enemy.xDistance / (agent.speed * enemy.zDistance); } }
    private bool arriveInPrecision = true;

    public PositionningTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
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

            enemy.destination = enemy.playerPosition + (att.intelligence * anticipation * anticipation + enemy.zDistance) * enemy.playerForward;
            enemy.destination = new Vector3(enemy.destination.x, enemy.destination.y, Mathf.Max(enemy.transform.position.z, enemy.destination.z));
        }
        else if (enemy.xDistance <= enemy.precision && enemy.toPlayerAngle > 5)
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
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Wait");
            arriveInPrecision = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (enemy.zDistance < att.positionningDist)
        {
            // Ready
            nextState = new ReadyTDS(enemy, agent, animator);
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
