using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private MainManager main;

    [Header("UI prefab")]
    [SerializeField] private Transform restartScreen;
    [SerializeField] private GameObject unlockWindow;

    [Header("Missions")]
    [SerializeField] private Mission teamMission;
    [SerializeField] private Mission zombieMission;
    [Space]
    [SerializeField] private Mission proMission;
    [SerializeField] private Mission starMission;
    [SerializeField] private Mission veteranMission;
    [SerializeField] private Mission legendMission;
    [Space]
    [SerializeField] private Mission rainMission;
    [SerializeField] private Mission fogMission;
    [Space]
    [SerializeField] private Mission bonusMission;
    [SerializeField] private Mission obstacleMission;
    [SerializeField] private Mission objectifMission;
    [SerializeField] private Mission weaponMission;



    public void CompleteMissions(GameData data, int wave)
    {
        ProgressionData pData = main.DataManager.progressionData;

        // Team
        if (pData.teamMode)
        {
            if (teamMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.teamMode = false;
                Unlock("team mode");
            }
        }
        // Zombie
        if (pData.zombieMode)
        {
            if (zombieMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.zombieMode = false;
                Unlock("zombie mode");
            }
        }

        // Pro
        if (pData.proDiff)
        {
            if (proMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.proDiff = false;
                Unlock("pro difficulty");
            }
        }
        // Star
        if (pData.starDiff)
        {
            if (starMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.starDiff = false;
                Unlock("star difficulty");
            }
        }
        if (pData.veteranDiff)
        {
            if (veteranMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.veteranDiff = false;
                Unlock("veteran difficulty");
            }
        }
        if (pData.legendDiff)
        {
            if (legendMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.legendDiff = false;
                Unlock("legend difficulty");
            }
        }

        // Rain
        if (pData.rainWeather)
        {
            if (rainMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.rainWeather = false;
                Unlock("rain weather");
            }
        }
        // Fog
        if (pData.fogWeather)
        {
            if (fogMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.fogWeather = false;
                Unlock("fog weather");
            }
        }

        // Bonus
        if (pData.bonusOpt)
        {
            if (bonusMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.bonusOpt = false;
                Unlock("bonus option");
            }
        }
        // Obstacle
        if (pData.obstacleOpt)
        {
            if (obstacleMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.obstacleOpt = false;
                Unlock("obstacle option");
            }
        }
        // Objectif
        if (pData.objectifOpt)
        {
            if (objectifMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.objectifOpt = false;
                Unlock("objectif option");
            }
        }
        // Weapon
        if (pData.weaponOpt)
        {
            if (weaponMission.CompleteMission(data, wave))
            {
                main.DataManager.progressionData.weaponOpt = false;
                Unlock("weapon option");
            }
        }
    }


    private void Unlock(string unlocked)
    {
        TextMeshProUGUI text = Instantiate(unlockWindow, restartScreen).GetComponentInChildren<TextMeshProUGUI>();

        text.text = unlocked;
    }
}
