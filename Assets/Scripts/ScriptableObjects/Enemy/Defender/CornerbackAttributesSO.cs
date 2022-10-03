using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/CornerbackAttribute", order = 1)]
public class CornerbackAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int)DefenderType.CORNERBACK; } }

    [Header("Cornerback Attributes")]
    [Tooltip("Mode of the interceptor :\n" +
        "TRUE = time --> base his attack on Attack time\n" +
        "FALSE = distance --> base his attack on Attack dist")]
    public bool modeTime;
    [Tooltip("Time before impact at which the interceptor attacks")]
    public float attackTime;
    [Space]
    [Tooltip("Precision in the attack process")]
    [Range(0.75f, 1.25f)] public float attackPrecision;
}
