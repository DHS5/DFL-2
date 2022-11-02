using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class DefenderAttributesSO : EnemyAttributesSO
{
    public enum DefenderType { WINGMAN, LINEMAN, CORNERBACK, LINEBACKER, SAFETY, TACKLE };

    [Header("Defenders attributes")]
    [Header("Defender behaviour parameters")]
    [Tooltip("Pourcentage of perfection of the trajectory")]
    [Range(0, 1)] public float intelligence;
    [Tooltip("Target distance in front of the player")]
    public float anticipation;
    [Space]
    [Tooltip("If Z-Dist > waitDist --> WAIT")]
    public float waitDist;
    [Tooltip("If RawDist < attackDist --> Attack")]
    public float attackDist;
    [Space]
    [Tooltip("If ToPlayerAngle < attackAngle --> Attack")]
    public float attackAngle;
}
