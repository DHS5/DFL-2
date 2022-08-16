using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safety : Defender
{
    public SafetyAttributesSO Att { get; private set; }

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as SafetyAttributesSO;

        currentState = new WaitSDS(this, navMeshAgent, animator);
    }
}
