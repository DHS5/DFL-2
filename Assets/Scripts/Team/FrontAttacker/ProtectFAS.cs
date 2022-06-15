using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProtectFAS : AttackerState
{
    new FrontAttacker attacker;

    public ProtectFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.PROTECT;

        attacker = _attacker;
    }

    public override void Enter()
    {
        base.Enter();

        agent.speed = attacker.player.controller.NormalSpeed;
    }

    public override void Update()
    {
        base.Update();

        attacker.destination = attacker.transform.position + attacker.playerDir * attacker.positionRadius;
        attacker.destination.x = Mathf.Clamp(attacker.destination.x, attacker.playerPos.x - attacker.positionRadius / 2, attacker.playerPos.x + attacker.positionRadius / 2);
        attacker.destination.z = Mathf.Clamp(attacker.destination.z, attacker.playerPos.z + attacker.positionRadius / 2, attacker.playerPos.z + attacker.positionRadius);

        if (attacker.hasDefender)
        {
            nextState = new DefendFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }
}
