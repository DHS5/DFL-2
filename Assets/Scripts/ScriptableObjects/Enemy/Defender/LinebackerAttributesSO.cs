using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/LinebackerAttribute", order = 1)]
public class LinebackerAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int)DefenderType.LINEBACKER; } }

    [Header("Linebacker attributes")]
    [Tooltip("X-Distance around the trajectories's intersection point")]
    public float precision;
    [Space]
    [Tooltip("If Z-Dist < positionningDist --> Chase/Attack")]
    public float positionningDist;
    [Space]
    [Tooltip("Precision in the attack process")]
    [Range(0.75f, 1.25f)] public float attackPrecision;
}
