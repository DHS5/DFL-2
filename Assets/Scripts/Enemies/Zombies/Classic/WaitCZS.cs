using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitCZS : ClassicZState
{
    public WaitCZS(ClassicZombie _enemy, NavMeshAgent _agent, Animator _animator) : base(_enemy, _agent, _animator)
    {
        name = EState.WAIT;

        enemy = _enemy;
    }


    public override void Update()
    {
        base.Update();

        enemy.destination = enemy.transform.position;


        if (enemy.playerOnField && enemy.zDistance < att.waitDist)
        {
            nextState = new ChaseCZS(enemy, agent, animator);
            stage = Event.EXIT;
        }
    }
}
