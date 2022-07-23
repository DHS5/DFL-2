using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallSZS : EnemyState
{
    new SleepingZombie enemy;

    private float animTime = 1.3f;

    public FallSZS(SleepingZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.FALL;

        enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Fall");

        enemy.destination = enemy.transform.position + 8 * agent.velocity.normalized;
    }

    public override void Update()
    {
        base.Update();


        if (Time.time > startTime + animTime)
        {
            animator.enabled = false;
        }
    }
}
