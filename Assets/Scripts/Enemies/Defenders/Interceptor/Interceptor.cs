using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Defender
{
    public InterceptorAttribute Att { get; private set; }

    [Tooltip("Mode of the interceptor :\n" +
        "TRUE = time --> base his attack on Attack time\n" +
        "FALSE = distance --> base his attack on Attack dist")]
    public bool modeTime;

    [Tooltip("Time before impact at which the interceptor attacks")]
    public float attackTime;

    protected override void Awake()
    {
        base.Awake();

        Att = Attribute.interceptorAtt;

        currentState = new WaitIDS(this, navMeshAgent, animator);
    }
}
