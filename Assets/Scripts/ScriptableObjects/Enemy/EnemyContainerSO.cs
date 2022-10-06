using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyContainer", menuName = "ScriptableObjects/Enemy/EnemyContainer", order = 1)]
public class EnemyContainerSO : ScriptableObject 
{
    public int startRank;

    [Header("Defenders")]
    public DefenderTypeArrays rookieD;
    public DefenderTypeArrays proD;
    public DefenderTypeArrays starD;
    public DefenderTypeArrays veteranD;
    public DefenderTypeArrays legendD;

    public DefenderTypeArrays GetArrays(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return rookieD;
            case 1:
                return proD;
            case 2:
                return starD;
            case 3:
                return veteranD;
            case 4:
                return legendD;
            default: return rookieD;
        }
    }

    [Header("Zombies")]
    public ZombieTypeArrays rookieZ;
    public ZombieTypeArrays proZ;
    public ZombieTypeArrays starZ;
    public ZombieTypeArrays veteranZ;
    public ZombieTypeArrays legendZ;

    public ZombieTypeArrays GetZArrays(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                return rookieZ;
            case 1:
                return proZ;
            case 2:
                return starZ;
            case 3:
                return veteranZ;
            case 4:
                return legendZ;
            default:
                return rookieZ;
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
