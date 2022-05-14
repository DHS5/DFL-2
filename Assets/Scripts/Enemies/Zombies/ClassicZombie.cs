using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicZombie : Zombie
{
    public override void ChasePlayer()
    {
        base.ChasePlayer();
        navMeshAgent.SetDestination(playerPosition);
    }

    private void LateUpdate()
    {
        if (player.GetComponent<PlayerGameplay>().isChasable && navMeshAgent.isOnNavMesh)
        {
            ChasePlayer();
        }
    }
}
