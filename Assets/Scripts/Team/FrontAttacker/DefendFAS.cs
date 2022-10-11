using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendFAS : FrontAttackerState
{
    public DefendFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Block");

        agent.speed = att.defenseSpeed;
        agent.angularSpeed = att.defenseRotSpeed;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + attacker.playerDir * att.anticipation + att.defenseDistMultiplier * attacker.playerTargetDist * attacker.player2TargetDir;


        if (attacker.targetPos.z < attacker.playerPos.z || attacker.playerTargetDist + 2 < attacker.playerDist)
        {
            nextState = new BackFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Block");

        attacker.UnTarget();
        agent.angularSpeed = att.rotationSpeed;
    }
}
