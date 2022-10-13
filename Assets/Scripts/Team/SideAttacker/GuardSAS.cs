using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardSAS : SideAttackerState
{
    public GuardSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("SideBlock");
        animator.SetFloat("Side", (int)att.Side);

        agent.speed = attacker.PlayerSpeed + att.defenseSpeed;
        agent.angularSpeed = att.defenseRotSpeed;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + AnticipationDir(att.anticipationType) * att.anticipation + att.defenseDistMultiplier * EnemyDir(att.anticipationType, att.anticipation);


        if (!attacker.IsThreat() || attacker.playerTargetDist + 2 < attacker.playerDist)
        {
            nextState = new BackSAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("SideBlock");
        animator.SetFloat("Side", 0f);

        attacker.UnTarget();
        agent.angularSpeed = att.rotationSpeed;
    }
}
