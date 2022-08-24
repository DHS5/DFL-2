using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyContainer", menuName = "ScriptableObjects/Enemy/EnemyContainer", order = 1)]
public class EnemyContainerSO : ScriptableObject 
{
    [Header("Defenders")]
    public DefenderTypeArrays easyD;
    public DefenderTypeArrays normalD;
    public DefenderTypeArrays hardD;

    public DefenderTypeArrays GetArrays(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return easyD;
            case 1:
                return normalD;
            default:
                return hardD;
        }
    }

    [Header("Zombies")]
    public ZombieTypeArrays easyZ;
    public ZombieTypeArrays normalZ;
    public ZombieTypeArrays hardZ;

    public ZombieTypeArrays GetZArrays(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return easyZ;
            case 1:
                return normalZ;
            default:
                return hardZ;
        }
    }
}

[System.Serializable]
public class DefenderTypeArrays
{
    public DefenderAttributesSO[] wingmen;
    public DefenderAttributesSO[] linemen;
}

[System.Serializable]
public class ZombieTypeArrays
{
    public ClassicZAttributesSO[] classic;
    public SleepingZAttributesSO[] sleeping;
    public BigZAttributesSO[] big;
}
