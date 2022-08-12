using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CornerbackState : EnemyState
{
    protected CornerbackAttributesSO att;

    new protected Cornerback enemy;

    public CornerbackState(Cornerback _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
