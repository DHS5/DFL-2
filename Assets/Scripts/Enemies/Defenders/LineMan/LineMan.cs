using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMan : Defender
{
    public LinemanAttributesSO Att { get; private set; }

    [HideInInspector] public float precision;

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as LinemanAttributesSO;

        precision = Random.Range(Att.precision / 2, Att.precision);

        currentState = new WaitLDS(this, navMeshAgent, animator);
    }
}
