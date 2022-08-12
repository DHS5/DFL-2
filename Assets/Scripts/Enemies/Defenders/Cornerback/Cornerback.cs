using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cornerback : Defender
{
    public CornerbackAttributesSO Att { get; private set; }

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as CornerbackAttributesSO;

        currentState = new WaitCDS(this, navMeshAgent, animator);
    }
}
