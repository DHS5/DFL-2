using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AttackerAttributesSO : ScriptableObject 
{
    public int level;

    public abstract AttackerType Type { get; }

    [Header("Physic parameters")]
    public Vector3 size;
    [Space]
    public float speed;
    public float acceleration;
    public int rotationSpeed;
    public bool autoBraking;
    [Space]
    public float back2PlayerSpeed;
    public float defenseSpeed;
    public float defenseRotSpeed;


    [Header("Behaviour parameters")]
    [Range(0, 1)] public float reactivity;
    public float anticipation;
    [Space]
    public float positionRadius;
    [Range(0, 1)] public float defenseDistMultiplier;
}


