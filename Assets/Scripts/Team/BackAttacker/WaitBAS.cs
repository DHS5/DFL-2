using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitBAS : BackAttackerState
{
    public WaitBAS(BackAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.WAIT;
    }

    public override void Update()
    {
        base.Update();

        if (attacker.player != null && attacker.player.gameplay.onField)
        {
            nextState = new ProtectBAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }
}
