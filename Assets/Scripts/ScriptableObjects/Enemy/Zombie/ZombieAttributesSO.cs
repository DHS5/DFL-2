using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieType { CLASSIC = 6, SLEEPING = 7, BIG = 8 };

public abstract class ZombieAttributesSO : EnemyAttributesSO
{
    [Header("Zombie attributes")]
    [Tooltip("If RawDist < attackDist --> Attack")]
    public float attackDist;
}


