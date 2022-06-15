using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendFAS : AttackerState
{
    new FrontAttacker attacker;

    public DefendFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;

        attacker = _attacker;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("Block");
    }


    public override void Update()
    {
        base.Update();

        animator.SetFloat("SpeedM", agent.velocity.magnitude / agent.speed);

        attacker.destination = attacker.playerPos + attacker.defenseDistMultiplier * attacker.playerTargetDist * attacker.player2TargetDir;


        if (attacker.targetPos.z < attacker.playerPos.z)
        {
            nextState = new BackFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("Block");
    }
}
