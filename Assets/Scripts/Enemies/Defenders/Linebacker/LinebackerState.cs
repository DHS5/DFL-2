using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LinebackerState : EnemyState
{
    protected LinebackerAttributesSO att;

    new protected LineBacker enemy;

    public LinebackerState(LineBacker _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
