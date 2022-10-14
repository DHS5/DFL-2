using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlockFAS : FrontAttackerState
{
    private bool blocked = false;

    private float BlockAngle
    {
        get { return Mathf.Abs(Vector3.Angle(-attacker.player2TargetDir, attacker.transform.position - attacker.targetPos)); }
    }
    private float TargetDist
    {
        get { return Vector3.Distance(attacker.transform.position, attacker.targetPos); }
    }

    public BlockFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.PlayerSpeed + att.defenseSpeed;
        agent.angularSpeed = att.defenseRotSpeed;
        agent.avoidancePriority = 0;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + att.defenseDistMultiplier * EnemyDir(att.anticipationType, att.anticipation);

        if (!blocked && attacker.playerPos.z < attacker.transform.position.z && TargetDist < att.blockDistance && BlockAngle < att.blockAngle)// && TargetAngle < att.blockAngle)
        {
            blocked = true;

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            animator.SetTrigger("StopBlock");
        }

        // If behind player, stop the block
        if (attacker.transform.position.z + att.blockLate < attacker.playerPos.z)
        {
            nextState = new BackFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("StopBlock");

        attacker.UnTarget();
        agent.isStopped = false;
        agent.angularSpeed = att.rotationSpeed;
        agent.avoidancePriority = 99;
    }
}
