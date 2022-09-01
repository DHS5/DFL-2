using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrontAttackerState : AttackerState
{
    new protected FrontAttacker attacker;

    protected FrontAttAttributesSO att;

    public FrontAttackerState(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        attacker = _attacker;

        att = attacker.Att;
    }
}
