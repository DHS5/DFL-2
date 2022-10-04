using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningLDS : LineManState
{
    private float preciseXDistance;

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
            animator.SetTrigger("Run");
            animator.ResetTrigger("Wait");
            agent.updateRotation = true;
            enemy.destination = enemy.playerPosition + (Mathf.Clamp(enemy.zDistance / att.waitDist, 0, Mathf.Max(0, 1 - att.positioningRatio)) + Mathf.Max(Mathf.Abs(enemy.xDistance / enemy.zDistance), att.positioningRatio)) * enemy.zDistance * PlayerDir;
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

        agent.updateRotation = true;
    }
}
