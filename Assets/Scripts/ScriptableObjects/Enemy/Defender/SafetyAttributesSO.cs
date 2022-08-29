using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/SafetyAttribute", order = 1)]
public class SafetyAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int) DefenderType.SAFETY; } }

    [Header("Safety attributes")]
    [Tooltip("Pourcentage of interpolation between player velocity and player forward")]
    [Range(0, 1)] public float precision;
    [Tooltip("If in the angle --> Wait")]
    public float waitAngle;
    [Space]
    [Tooltip("If RawDist < chaseDist --> Chase / else --> Intercept")]
    public float chaseDist;
    [Space]
    [Tooltip("If in the precision cone and distance < patience --> attack")]
    public float patience;
}
