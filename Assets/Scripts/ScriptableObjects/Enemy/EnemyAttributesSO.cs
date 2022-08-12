using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class EnemyAttributesSO : ScriptableObject 
{
    public int level;

    public abstract int Type { get; }

    [Header("Physic parameters")]
    public Vector3 size;
    [Space]
    public float speed;
    public float acceleration;
    public float attackSpeed;
    public int rotationSpeed;
    public bool autoBraking;
    [Space]
    [Range(0, 1)] public float reactivity;
}


