using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAttacker : Attacker
{
    protected override void Awake()
    {
        base.Awake();

        type = AttackerType.FRONT;

        currentState = new WaitFAS(this, navMeshAgent, animator);
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


    // ### Functions ###

    public override Vector3 ClampInZone(Vector3 destination)
    {
        destination.z = Mathf.Clamp(destination.z, playerPos.z + positionRadius / 2, playerPos.z + positionRadius);
        destination.x = Mathf.Clamp(destination.x, playerPos.x - ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2), playerPos.x + ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2));
        return destination;
    }

    public override bool InZone(Vector3 destination)
    {
        if (destination.z < playerPos.z + positionRadius && destination.z > playerPos.z + positionRadius / 2)
            if (destination.x < playerPos.x + ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2) && destination.x > playerPos.x - ((destination.z - playerPos.z) * Mathf.Sqrt(2) / 2))
                return true;
        return false;
    }
}
