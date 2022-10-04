using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningTDS : TackleState
{
    private float anticipation { get { return att.anticipation * enemy.playerSpeed * enemy.xDistance / (agent.speed * enemy.zDistance); } }

    public PositionningTDS(Tackle _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.POSITIONNING;
    }


    public override void Update()
    {
        base.Update();


        if (enemy.xDistance > att.precision)
        {
            animator.SetTrigger("Run");
            animator.ResetTrigger("Wait");

            agent.isStopped = false;
            agent.updateRotation = true;

            enemy.destination = enemy.playerPosition + (anticipation * anticipation + enemy.zDistance) * enemy.playerForward;
            enemy.destination = new Vector3(enemy.destination.x, enemy.destination.y, Mathf.Max(enemy.transform.position.z, enemy.destination.z));
        }
        else
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Wait");

            agent.isStopped = true;
            agent.updateRotation = false;

            enemy.transform.rotation =
                Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(enemy.toPlayerDirection), att.rotationSpeed * Time.deltaTime);
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

        agent.isStopped = false;
        agent.updateRotation = true;
    }
}
