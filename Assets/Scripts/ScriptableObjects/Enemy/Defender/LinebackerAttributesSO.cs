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
}
