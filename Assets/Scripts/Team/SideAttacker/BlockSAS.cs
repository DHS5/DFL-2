using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlockSAS : SideAttackerState
{
    private bool blocked = false;

    private float TargetDist
    {
        get { return Vector3.Distance(attacker.transform.position, attacker.targetPos); }
    }
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

    public BlockSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.PlayerSpeed + att.defenseSpeed;
        agent.angularSpeed = att.defenseRotSpeed;
    }


    public override void Update()
    {
        base.Update();

        //attacker.destination = attacker.playerPos + AnticipationDir(att.anticipationType) * att.anticipation + att.defenseDistMultiplier * EnemyDir(att.anticipationType, att.anticipation);
        //attacker.destination = attacker.targetPos + att.anticipation * attacker.targetDir;
        attacker.destination = attacker.targetPos + (att.anticipationType ? att.anticipation : TargetDist * att.defenseDistMultiplier) * attacker.targetDir;

        if (!blocked && attacker.playerPos.z < attacker.transform.position.z && TargetDist < att.blockDistance && InTheMiddle)
        {
            blocked = true;

            agent.isStopped = true;
            agent.updateRotation = false;
            agent.velocity = Vector3.zero;

            attacker.transform.LookAt(attacker.target.transform);

            animator.SetTrigger("StopBlock");
        }
        else if ((!blocked && !attacker.IsThreat()) || attacker.transform.position.z + att.blockLate < attacker.playerPos.z)
        {
            nextState = new BackSAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("StopBlock");

        attacker.UnTarget();
        agent.isStopped = false;
        agent.updateRotation = true;
        agent.angularSpeed = att.rotationSpeed;
    }
}
