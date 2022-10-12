using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SafetyState : EnemyState
{
    protected SafetyAttributesSO att;

    new protected Safety enemy;

    protected Vector3 PlayerDir { get { return Vector3.Slerp(enemy.playerVelocity, enemy.playerForward, att.intelligence); } }

    public SafetyState(Safety _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
