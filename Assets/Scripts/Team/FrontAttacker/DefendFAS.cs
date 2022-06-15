using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefendFAS : AttackerState
{
    new FrontAttacker attacker;

    public DefendFAS(FrontAttacker _attacker, NavMeshAgent _agent, Animator _animator) : base(_attacker, _agent, _animator)
    {
        name = AState.DEFEND;

        attacker = _attacker;
    }


    public override void Update()
    {
        base.Update();

        animator.SetFloat("SpeedM", agent.velocity.magnitude / agent.speed);


        if (attacker.targetPos.z < attacker.playerPos.z)
        {
            nextState = new BackFAS(attacker, agent, animator);
            stage = Event.EXIT;
        }
    }
}
