using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SideAttackerState : AttackerState
{
    new protected SideAttacker attacker;

    protected SideAttAttributesSO att;

    public SideAttackerState(SideAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        attacker = _attacker;

        att = attacker.Att;
    }
}