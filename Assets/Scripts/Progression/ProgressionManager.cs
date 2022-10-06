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
    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private TMP_Dropdown weatherDropdown;
    [Space]
    [Space]
    [SerializeField] private Lock teamLock;
    [SerializeField] private Lock zombieLock;

    private Lock[] difficultyLocks;

    private Lock[] weatherLocks;
    [Space]
    [SerializeField] private Lock bonusLock;
    [SerializeField] private Lock[] obstacleLocks;
    [SerializeField] private Lock[] objectifLocks;
    [SerializeField] private Lock weaponLock;



    readonly string proDiffText = "Reach wave 6 in Defender or Team mode to unlock pro difficulty";
    readonly string starDiffText = "Reach wave 6 in pro difficulty in Defender or Team mode to unlock star difficulty";
    readonly string veteranDiffText = "Reach wave 6 in star difficulty in Defender or Team mode to unlock veteran difficulty";
    readonly string legendDiffText = "Reach wave 6 in veteran difficulty in Defender or Team mode to unlock legend difficulty";
    
    readonly string rainWeatherText = "Reach wave 5 in objectif option to unlock rain wheather";
    readonly string fogWeatherText = "Reach wave 5 in rain wheather to unlock fog wheather";


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
    private void SetProgressionData()
    {
        main.DataManager.progressionData = progressionData;
    }

    public void ApplyProgressionData()
    {
        teamLock.Locked = progressionData.teamMode;
        zombieLock.Locked = progressionData.zombieMode;

        bonusLock.Locked = progressionData.bonusOpt;
        foreach (Lock l in obstacleLocks)
            l.Locked = progressionData.obstacleOpt;
        foreach (Lock l in objectifLocks)
            l.Locked = progressionData.objectifOpt;
        weaponLock.Locked = progressionData.weaponOpt;
    }



    private void GetLocksFromDropdown(ref Lock[] locks, TMP_Dropdown dropdown)
    {
        locks = dropdown.GetComponentsInChildren<Lock>();
    }  



    public void ApplyDropdownLocks()
    {
        GetLocksFromDropdown(ref difficultyLocks, difficultyDropdown);
        GetLocksFromDropdown(ref weatherLocks, weatherDropdown);

        if (difficultyLocks.Length > 4)
        {
            difficultyLocks[0].Locked = false;
            difficultyLocks[1].Locked = progressionData.proDiff; difficultyLocks[1].text.text = proDiffText;
            difficultyLocks[2].Locked = progressionData.starDiff; difficultyLocks[2].text.text = starDiffText;
            difficultyLocks[3].Locked = progressionData.veteranDiff; difficultyLocks[3].text.text = veteranDiffText;
            difficultyLocks[4].Locked = progressionData.legendDiff; difficultyLocks[4].text.text = legendDiffText;
        }
        
        if (weatherLocks.Length > 2)
        {
            weatherLocks[0].Locked = false;
            weatherLocks[1].Locked = progressionData.rainWeather; weatherLocks[1].text.text = rainWeatherText;
            weatherLocks[2].Locked = progressionData.fogWeather; weatherLocks[2].text.text = fogWeatherText;
        }
    }
}
