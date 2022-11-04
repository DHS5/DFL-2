using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBacker : Defender
{
    public LinebackerAttributesSO Att { get; private set; }

    [HideInInspector] public float precision;

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as LinebackerAttributesSO;

        precision = Random.Range(Att.precision / 2, Att.precision);

        currentState = new WaitLBDS(this, navMeshAgent, animator);
    }
}
