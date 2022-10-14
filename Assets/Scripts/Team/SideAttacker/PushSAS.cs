using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushSAS : SideAttackerState
{
    private float TargetDist
    {
        get { return Vector3.Distance(attacker.transform.position, attacker.targetPos); }
    }

    public PushSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
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
        agent.avoidancePriority = 0;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.targetPos + (att.anticipationType ? att.anticipation : TargetDist * att.defenseDistMultiplier) * attacker.targetDir;


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
        agent.avoidancePriority = 99;
    }
}
