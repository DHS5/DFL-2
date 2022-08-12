using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/LinemanAttribute", order = 1)]
public class LinemanAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int)DefenderType.LINEMAN; } }

    [Header("Lineman attributes")]
    [Tooltip("X-Distance around the trajectories's intersection point")]
    public float precision;
    [Space]
    [Tooltip("If Z-Dist < positionningDist --> Chase/Attack")]
    public float positionningDist;
}
