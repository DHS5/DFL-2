using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingZombie : Zombie
{
    public override void ChasePlayer()
    {
        base.ChasePlayer();
        if (distance < chaseRadius && navMeshAgent.remainingDistance < 5)
        {
            navMeshAgent.SetDestination(playerPosition + playerDirection * intelligence);
        }
    }

    private void LateUpdate()
    {
        if (player.GetComponent<PlayerGameplay>().isChasable && navMeshAgent.isOnNavMesh)
        {
            ChasePlayer();
        }
    }
}
