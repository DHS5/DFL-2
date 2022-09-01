using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackAttackerState : AttackerState
{
    new protected BackAttacker attacker;

    protected BackAttAttributesSO att;

    public BackAttackerState(BackAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        attacker = _attacker;

        att = attacker.Att;
    }
}
