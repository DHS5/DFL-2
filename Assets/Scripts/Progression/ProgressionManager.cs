using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class ProgressionManager : MonoBehaviour
{
    private MenuMainManager main;
    
    
    [HideInInspector] public ProgressionData progressionData;

    [Header("UI components")]
    [SerializeField] private Lock[] modeLocks;
    [SerializeField] private Lock[] difficultyLocks;
    [SerializeField] private Lock[] weatherLocks;
    [Space]
    [SerializeField] private Lock bonusLock;
    [SerializeField] private Lock[] obstacleLocks;
    [SerializeField] private Lock[] objectifLocks;
    [SerializeField] private Lock weaponLock;


    readonly string teamModeText = "Reach wave 5 in any mode (except drill) to unlock team mode";
    readonly string zombieModeText = "Reach wave 5 in team mode to unlock zombie mode";

    readonly string proDiffText = "Reach wave 6 in Defender or Team mode to unlock pro difficulty";
    readonly string starDiffText = "Reach wave 6 in pro difficulty in Defender or Team mode to unlock star difficulty";
    readonly string veteranDiffText = "Reach wave 6 in star difficulty in Defender or Team mode to unlock veteran difficulty";
    readonly string legendDiffText = "Reach wave 6 in veteran difficulty in Defender or Team mode to unlock legend difficulty";
    
    readonly string rainWeatherText = "Reach wave 4 in objectif option to unlock rain wheather";
    readonly string fogWeatherText = "Reach wave 5 in rain wheather to unlock fog wheather";

    readonly string bonusOptionText = "Reach wave 3 in any mode to unlock bonus option";
    readonly string obstacleOptionText = "Reach wave 5 in zombie mode to unlock obstacles option";
    readonly string objectifOptionText = "Reach wave 5 in objectif drill to unlock objectif option";
    readonly string weaponOptionText = "Reach wave 3 in obstacle zombie to unlock weapon option";


    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }

    public void LoadProgression()
    {
        GetProgressionData();
        ApplyProgressionData();
    }

    private void GetProgressionData()
    {
        progressionData = main.DataManager.progressionData;
    }

    public void ApplyProgressionData()
    {
        modeLocks[0].ApplyLockInfos(progressionData.teamMode, teamModeText);
        modeLocks[1].ApplyLockInfos(progressionData.zombieMode, zombieModeText);

        difficultyLocks[0].ApplyLockInfos(progressionData.proDiff, proDiffText);
        difficultyLocks[1].ApplyLockInfos(progressionData.starDiff, starDiffText);
        difficultyLocks[2].ApplyLockInfos(progressionData.veteranDiff, veteranDiffText);
        difficultyLocks[3].ApplyLockInfos(progressionData.legendDiff, legendDiffText);

        weatherLocks[0].ApplyLockInfos(progressionData.rainWeather, rainWeatherText);
        weatherLocks[1].ApplyLockInfos(progressionData.fogWeather, fogWeatherText);

        bonusLock.ApplyLockInfos(progressionData.bonusOpt, bonusOptionText);
        foreach (Lock l in obstacleLocks)
            l.ApplyLockInfos(progressionData.obstacleOpt, obstacleOptionText);
        foreach (Lock l in objectifLocks)
            l.ApplyLockInfos(progressionData.objectifOpt, objectifOptionText);
        weaponLock.ApplyLockInfos(progressionData.weaponOpt, weaponOptionText);
    }
}
