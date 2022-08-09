using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingMan : Defender
{
    public WingManAttribute Att { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Att = Attribute.wingmanAtt;

        currentState = new WaitWDS(this, navMeshAgent, animator);
    }
}
