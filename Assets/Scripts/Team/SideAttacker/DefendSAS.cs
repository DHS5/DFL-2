using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendSAS : AttackerState
{
    new SideAttacker attacker;

    private int side;

    public DefendSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator, int _side) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;

        attacker = _attacker;

        side = _side;
    }

    public override void Enter()
    {
        base.Enter();

        animator.SetTrigger("SideBlock");
        animator.SetFloat("Side", side);

        agent.speed = attacker.defenseSpeed;
    }


    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.playerPos + attacker.defenseDistMultiplier * attacker.playerTargetDist * attacker.player2TargetDir;


        if (!attacker.IsThreat())
        {
            nextState = new BackSAS(attacker, agent, animator, side);
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();

        animator.ResetTrigger("SideBlock");
        animator.SetFloat("Side", 0f);

        attacker.UnTarget();
    }
}
