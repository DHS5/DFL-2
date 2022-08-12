using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ClassicZAttribute", menuName = "ScriptableObjects/Enemy/Zombie/ClassicZAttribute", order = 1)]
public class ClassicZAttributesSO : ZombieAttributesSO
{
    public override int Type { get { return (int) ZombieType.CLASSIC; } }

    [Header("Classic Zombie attributes")]
    [Tooltip("If Z-Dist > waitDist --> WAIT")]
    public float waitDist;
}


