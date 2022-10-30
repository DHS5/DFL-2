using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Enemy
{
    public DefenderAttributesSO Attribute { get; private set; }

    private void Update()
    {
        if (Attribute.reactivity == 0 && playerG.onField && !gameOver)
        {
            ChasePlayer();
        }
    }


    // ### Functions ###

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        if (att != null)
            Attribute = att as DefenderAttributesSO;
    }


    /// <summary>
    /// Gives the NavMeshAgent his destination
    /// </summary>
    public override void ChasePlayer()
    {
        base.ChasePlayer();

        currentState = currentState.Process();

        if (playerG.onField && !gameOver)
        {
            navMeshAgent.SetDestination(destination);
        }

        if (Attribute.reactivity != 0 && !gameOver)
        {
            Invoke(nameof(ChasePlayer), Attribute.reactivity);
        }
    }

    public override void GameOver()
    {
        base.GameOver();

        DestroyImmediate(audioSource);
    }
}
