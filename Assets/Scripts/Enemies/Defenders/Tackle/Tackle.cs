using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : Defender
{
    public TackleAttributesSO Att { get; private set; }

    [HideInInspector] public float precision;

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as TackleAttributesSO;

        precision = Random.Range(0, Att.precision * Att.intelligence);

        currentState = new WaitTDS(this, navMeshAgent, animator);
    }
}
