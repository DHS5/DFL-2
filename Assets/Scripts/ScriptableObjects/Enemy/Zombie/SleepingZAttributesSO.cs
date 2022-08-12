using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SleepingZAttribute", menuName = "ScriptableObjects/Enemy/Zombie/SleepingZAttribute", order = 1)]
public class SleepingZAttributesSO : ZombieAttributesSO
{
    public override int Type { get { return (int) ZombieType.SLEEPING; } }

    [Header("Sleeping Zombie attributes")]
    [Tooltip("Target distance in front of the player if interception impossible")]
    public float anticipation;
}


