using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ZombieType { CLASSIC = 4, SLEEPING = 5 };

public abstract class ZombieAttributesSO : EnemyAttributesSO
{
    [Header("Zombie attributes")]
    [Tooltip("If RawDist < attackDist --> Attack")]
    public float attackDist;
}


