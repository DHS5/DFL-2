using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendBAS : AttackerState
{
    new BackAttacker attacker;

    public DefendBAS(BackAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;

        attacker = _attacker;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("BackBlock");

        agent.speed = attacker.defenseSpeed;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + attacker.defenseDistMultiplier * attacker.playerTargetDist * attacker.player2TargetDir;


        if (attacker.targetPos.z < attacker.playerPos.z - attacker.ProtectionRadius)
        {
            nextState = new BackBAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("BackBlock");

        attacker.UnTarget();
    }
}
