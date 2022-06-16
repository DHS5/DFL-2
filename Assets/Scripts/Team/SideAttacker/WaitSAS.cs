using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitSAS : AttackerState
{
    new SideAttacker attacker;

    private int side;

    public WaitSAS(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator, int _side) : base(_attacker, _agent, _animator)
    {
        name = AState.WAIT;

        attacker = _attacker;

        side = _side;
    }

    public override void Update()
    {
        base.Update();

        if (attacker.player != null && attacker.player.gameplay.onField)
        {
            nextState = new ProtectSAS(attacker, agent, animator, side);
            stage = Event.EXIT;
        }
    }
}
