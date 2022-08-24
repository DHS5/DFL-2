using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BigZAttribute", menuName = "ScriptableObjects/Enemy/Zombie/BigZAttribute", order = 1)]
public class BigZAttributesSO : ZombieAttributesSO
{
    public override int Type { get { return (int) ZombieType.BIG; } }

    [Header("Big Zombie attributes")]
    [Tooltip("If Z-Dist > waitDist --> WAIT")]
    public float waitDist;
    [Tooltip("If xDist < waitXRange")]
    public float waitXRange;
}


