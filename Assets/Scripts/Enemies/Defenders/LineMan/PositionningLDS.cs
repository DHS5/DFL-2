using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionningLDS : EnemyState
{
    public PositionningLDS(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
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
            enemy.destination = enemy.playerPosition + enemy.intelligence * enemy.zDistance * enemy.playerLookDirection;
        }
        else
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Wait");
            enemy.destination = enemy.transform.position;
            agent.updateRotation = false;
            enemy.transform.rotation =
                Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(enemy.toPlayerDirection), 10 * Time.deltaTime);
        }

        if (enemy.zDistance < enemy.positionningDist)
        {
            // Attack
            if (enemy.rawDistance < enemy.attackDist)
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
    }
}
