using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safety : Defender
{
    public LineBackerAttribute Att { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        precision = Random.Range(0, precision * intelligence);

        Att = Attribute.linebackerAtt;

        currentState = new WaitSDS(this, navMeshAgent, animator);
    }
}
