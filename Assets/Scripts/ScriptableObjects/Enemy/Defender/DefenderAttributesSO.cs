using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DefenderType { WINGMAN, LINEMAN, INTERCEPTOR, LINEBACKER };

[CreateAssetMenu(fileName = "DefenderAttribute", menuName = "ScriptableObjects/Enemy/Defender/DefenderAttribute", order = 1)]
public class DefenderAttributesSO : EnemyAttributeSO
{
    public DefenderType type;
    public override int Type { get { return (int)type; } }

    [Header("Physic parameters")]
    public Vector3 size;
    [Space]
    public float speed;
    public float acceleration;
    public float attackSpeed;
    [Space]
    [Range(0, 1)] public float reactivity;

    [Header("Attributes")]
    public WingManAttribute wingmanAtt;
    public LineManAttribute linemanAtt;
    public InterceptorAttribute interceptorAtt;
    public LineBackerAttribute linebackerAtt;
}

[System.Serializable]
public class WingManAttribute
{
    [Tooltip("Target distance in front of the player")]
    public float anticipation;
    [Tooltip("Pourcentage of perfection of the interception trajectory")]
    [Range(0, 1)] public float intelligence;
    [Tooltip("If in the angle --> Chase")]
    public float chaseAngle;

    [Tooltip("If Z-Dist > waitDist --> WAIT")]
    public float waitDist;
    [Tooltip("If RawDist < chaseDist --> Chase / else --> Intercept")]
    public float chaseDist;
    [Tooltip("If RawDist < attackDist --> Attack")]
    public float attackDist;

    public float patience;
    public bool patient;
}
public class LineManAttribute
{

}
public class InterceptorAttribute
{

}
public class LineBackerAttribute
{

}
