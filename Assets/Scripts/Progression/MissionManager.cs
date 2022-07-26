using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private MainManager main;


    [SerializeField] private Mission teamMission;
    [SerializeField] private Mission zombieMission;
    [Space]
    [SerializeField] private Mission normalMission;
    [SerializeField] private Mission hardMission;
    [Space]
    [SerializeField] private Mission rainMission;
    [SerializeField] private Mission fogMission;
    [Space]
    [SerializeField] private Mission bonusMission;
    [SerializeField] private Mission obstacleMission;
    [SerializeField] private Mission objectifMission;
    [SerializeField] private Mission weaponMission;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    public void CompleteMissions(GameData data, int wave)
    {
        ProgressionData pData = main.DataManager.progressionData;

        // Team
        Debug.Log("team");
        if (pData.teamMode)
        {
            if (teamMission.CompleteMission(data, wave))
                main.DataManager.progressionData.teamMode = false;
        }
        // Zombie
        Debug.Log("zomb");
        if (pData.zombieMode)
        {
            if (zombieMission.CompleteMission(data, wave))
                main.DataManager.progressionData.zombieMode = false;
        }

        // Normal
        Debug.Log("norm");
        if (pData.normalDiff)
        {
            if (normalMission.CompleteMission(data, wave))
                main.DataManager.progressionData.normalDiff = false;
        }
        // Hard
        Debug.Log("hard");
        if (pData.hardDiff)
        {
            if (hardMission.CompleteMission(data, wave))
                main.DataManager.progressionData.hardDiff = false;
        }

        // Rain
        Debug.Log("rain");
        if (pData.rainWeather)
        {
            if (rainMission.CompleteMission(data, wave))
                main.DataManager.progressionData.rainWeather = false;
        }
        // Fog
        Debug.Log("fog");
        if (pData.fogWeather)
        {
            if (fogMission.CompleteMission(data, wave))
                main.DataManager.progressionData.fogWeather = false;
        }

        // Bonus
        if (pData.bonusOpt)
        {
            if (bonusMission.CompleteMission(data, wave))
                main.DataManager.progressionData.bonusOpt = false;
        }
        // Obstacle
        Debug.Log("obs");
        if (pData.obstacleOpt)
        {
            if (obstacleMission.CompleteMission(data, wave))
                main.DataManager.progressionData.obstacleOpt = false;
        }
        // Objectif
        Debug.Log("obj");
        if (pData.objectifOpt)
        {
            if (objectifMission.CompleteMission(data, wave))
                main.DataManager.progressionData.objectifOpt = false;
        }
        // Weapon
        Debug.Log("weap");
        if (pData.weaponOpt)
        {
            if (weaponMission.CompleteMission(data, wave))
                main.DataManager.progressionData.weaponOpt = false;
        }
    }
}
