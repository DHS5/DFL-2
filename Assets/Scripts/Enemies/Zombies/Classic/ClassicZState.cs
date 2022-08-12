using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClassicZState : EnemyState
{
    protected ClassicZAttributesSO att;

    new protected ClassicZombie enemy;

    public ClassicZState(ClassicZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
