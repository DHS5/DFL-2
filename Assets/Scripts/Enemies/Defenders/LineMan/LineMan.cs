using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMan : Defender
{
    public LineManAttribute Att { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        precision = Random.Range(0, precision * intelligence);

        Att = Attribute.linemanAtt;

        currentState = new WaitLDS(this, navMeshAgent, animator);
    }
}
