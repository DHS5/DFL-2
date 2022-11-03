using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameOverES : EnemyState
{
    public GameOverES(Enemy _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.GAMEOVER;
    }

    public override void Enter()
    {
        base.Enter();

        agent.isStopped = true;
        agent.ResetPath();
    }
}
