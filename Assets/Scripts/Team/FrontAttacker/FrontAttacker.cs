using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAttacker : Attacker
{
    protected override void Awake()
    {
        base.Awake();

        type = AttackerType.FRONT;

        currentState = new ProtectFAS(this, navMeshAgent, animator);
    }


    public override void ProtectPlayer()
    {
        base.ProtectPlayer();

        currentState = currentState.Process();

        if (player.gameplay.onField && !gameOver)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (reactivity != 0)
        {
            Invoke(nameof(ProtectPlayer), reactivity);
        }
    }


    private void Update()
    {
        if (reactivity == 0 && player.gameplay.onField && !gameOver)
        {
            ProtectPlayer();
        }
    }
}
