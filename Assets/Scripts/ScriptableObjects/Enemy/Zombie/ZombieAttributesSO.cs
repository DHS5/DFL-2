using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieType { CLASSIC, SLEEPING };

[CreateAssetMenu(fileName = "ZombieAttribute", menuName = "ScriptableObjects/Enemy/Zombie/ZombieAttribute", order = 1)]
public class ZombieAttributesSO : EnemyAttributeSO
{
    public ZombieType type;
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


