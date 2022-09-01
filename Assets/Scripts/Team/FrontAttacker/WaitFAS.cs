using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitFAS : FrontAttackerState
{
    public WaitFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.WAIT;
    }

    public override void Update()
    {
        base.Update();

        if (attacker.player != null && attacker.player.gameplay.onField)
        {
            nextState = new ProtectFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }
}
