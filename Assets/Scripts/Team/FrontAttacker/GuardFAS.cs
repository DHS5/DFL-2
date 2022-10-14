using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardFAS : FrontAttackerState
{
    private bool InTheMiddle
    {
        get
        {
            Vector3 aPos = attacker.transform.position;
            Vector3 tPos = attacker.targetPos;
            Vector3 pPos = attacker.playerPos;
            return aPos.x < Mathf.Max(tPos.x, pPos.x) && aPos.x > Mathf.Min(tPos.x, pPos.x) && aPos.z < Mathf.Max(tPos.z, pPos.z) && aPos.z > Mathf.Min(tPos.z, pPos.z);
        }
    }

    public GuardFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Block");

        agent.speed = attacker.PlayerSpeed + att.defenseSpeed;
        agent.angularSpeed = att.defenseRotSpeed;
        agent.avoidancePriority = 0;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + AnticipationDir(att.anticipationType) * att.anticipation + att.defenseDistMultiplier * EnemyDir(att.anticipationType, att.anticipation);


        if (attacker.targetPos.z < attacker.playerPos.z || attacker.playerTargetXDist > Mathf.Max(5, attacker.playerTargetZDist) || attacker.playerTargetDist + 2 < attacker.playerDist)
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
        agent.avoidancePriority = 99;
    }
}
