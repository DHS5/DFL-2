using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SleepingZState : EnemyState
{
    protected SleepingZAttributesSO att;

    new protected SleepingZombie enemy;

    public SleepingZState(SleepingZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        enemy = _enemy;

        att = enemy.Att;
    }
}
