using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAttacker : Attacker
{
    protected override void Awake()
    {
        base.Awake();

        type = AttackerType.BACK;

        currentState = new WaitBAS(this, navMeshAgent, animator);
    }


    public override void ProtectPlayer()
    {
        base.ProtectPlayer();

        currentState = currentState.Process();

        if (player.gameplay.onField && !gameOver)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (reactivity != 0 && !gameOver)
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
        destination.z = Mathf.Clamp(destination.z, playerPos.z - positionRadius, playerPos.z - positionRadius / 2);
        destination.x = Mathf.Clamp(destination.x, playerPos.x - ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2), playerPos.x + ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2));
        return destination;
    }

    public override bool InZone(Vector3 destination)
    {
        if (destination.z > playerPos.z - positionRadius && destination.z < playerPos.z - positionRadius / 2)
            if (destination.x < playerPos.x + ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2) && destination.x > playerPos.x - ((playerPos.z - destination.z) * Mathf.Sqrt(2) / 2))
                return true;
        return false;
    }
}
