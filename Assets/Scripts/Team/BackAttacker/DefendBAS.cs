using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendBAS : BackAttackerState
{
    public DefendBAS(BackAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("BackBlock");

        agent.speed = att.defenseSpeed;
        agent.angularSpeed = att.defenseRotSpeed;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + attacker.playerDir * att.anticipation + att.defenseDistMultiplier * attacker.playerTargetDist * attacker.player2TargetDir;


        if (attacker.targetPos.z < attacker.playerPos.z - attacker.ProtectionRadius || attacker.playerTargetDist + 2 < attacker.playerDist)
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
        agent.angularSpeed = att.rotationSpeed;
    }
}
