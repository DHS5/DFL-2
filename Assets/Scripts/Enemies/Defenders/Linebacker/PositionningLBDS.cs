using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningLBDS : LinebackerState
{
    public PositionningLBDS(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.POSITIONNING;
    }

    public override void Update()
    {
        base.Update();


        if (enemy.xDistance > enemy.precision)
        {
            animator.SetTrigger("Run");
            animator.ResetTrigger("Wait");
            agent.updateRotation = true;
            enemy.destination = enemy.playerPosition + Mathf.Max(Mathf.Abs(enemy.xDistance / enemy.zDistance), 1) * enemy.zDistance * enemy.playerForward;
        }
        else
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Wait");
            enemy.destination = enemy.transform.position;
            agent.updateRotation = false;
            enemy.transform.rotation =
                Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(enemy.toPlayerDirection), att.rotationSpeed * Time.deltaTime);
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
