using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WingManState : EnemyState
{
    protected WingmanAttributesSO att;

    new protected WingMan enemy;

    protected Vector3 PlayerDir { get { return Vector3.Lerp(enemy.playerVelocity, enemy.playerForward, att.intelligence); } }

    public WingManState(WingMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
