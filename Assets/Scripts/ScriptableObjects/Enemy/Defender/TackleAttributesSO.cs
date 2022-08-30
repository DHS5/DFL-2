using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/TackleAttribute", order = 1)]
public class TackleAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int)DefenderType.TACKLE; } }

    [Header("Tackle attributes")]
    [Tooltip("X-Distance around the trajectories's intersection point")]
    public float precision;
    [Space]
    [Tooltip("If Z-Dist < positionningDist --> Ready")]
    public float positionningDist;
    [Tooltip("If Z-Dist < readyDist --> Tired/Attack")]
    public float readyDist;
}
