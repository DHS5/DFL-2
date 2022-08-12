using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingMan : Defender
{
    public WingmanAttributesSO Att { get; private set; }

    public override void GetAttribute(EnemyAttributesSO att)
    {
        base.GetAttribute(att);

        Att = att as WingmanAttributesSO;

        currentState = new WaitWDS(this, navMeshAgent, animator);
    }
}
