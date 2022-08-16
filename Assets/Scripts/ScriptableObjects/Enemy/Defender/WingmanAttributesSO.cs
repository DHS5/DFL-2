using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "_AttSO", menuName = "ScriptableObjects/Enemy/Defender/WingmanAttribute", order = 1)]
public class WingmanAttributesSO : DefenderAttributesSO
{
    public override int Type { get { return (int) DefenderType.WINGMAN; } }

    [Header("Wingman attributes")]
    [Tooltip("If in the angle --> Chase")]
    public float chaseAngle;
    [Space]
    [Tooltip("If RawDist < chaseDist --> Chase / else --> Intercept")]
    public float chaseDist;
}
