using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMan : Defender
{
    /// <summary>
    /// Gives the NavMeshAgent his destination
    /// </summary>
    public override void ChasePlayer()
    {
        base.ChasePlayer();
        // Checks if the player is chasable
        if (player.GetComponent<PlayerGameplay>().isChasable)
        {
            // Sets the reactivity time to normal
            reactivityM = 1f;

            // If the player is in the attention radius
            if (distance <= attentionRadius)
            {
                // If the enemy is close enough --> go directly to the player
                if (distance <= attackRadius)
                {
                    destination = playerPosition;
                    // Increase the reactivity time
                    reactivityM = 0f;
                    if (!attackSpeed)
                    {
                        // Increase the speed
                        attackSpeed = true;
                        navMeshAgent.speed *= attackSpeedM;
                        Invoke(nameof(NormalSpeed), 0.1f * attackRadius);
                    }
                }

                // The enemy tries to anticipate the player's future position given his intelligence
                else
                    destination = playerPosition + playerDirection * distance;


                // Sets the agent's destination
                navMeshAgent.SetDestination(destination);
            }

            // Invoke recursively the method given the enemy's reactivity
            Invoke(nameof(ChasePlayer), reactivity * reactivityM);
        }
    }
}
