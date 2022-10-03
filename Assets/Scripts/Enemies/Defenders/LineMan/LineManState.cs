using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineManState : EnemyState
{
    protected LinemanAttributesSO att;

    new protected LineMan enemy;

    protected Vector3 PlayerDir { get { return Vector3.Lerp(enemy.playerLookDirection, enemy.playerForward, att.intelligence); } }

    public LineManState(LineMan _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
