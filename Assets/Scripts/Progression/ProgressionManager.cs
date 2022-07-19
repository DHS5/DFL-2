using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



public class ProgressionManager : MonoBehaviour
{
    private DataManager dataManager;
    
    
    [HideInInspector] public ProgressionData progressionData;

    [Header("UI components")]
    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private TMP_Dropdown wheatherDropdown;
    [Space]
    [Space]
    [SerializeField] private Lock teamLock;
    [SerializeField] private Lock zombieLock;

    private Lock[] difficultyLocks;

    private Lock[] wheatherLocks;
    [Space]
    [SerializeField] private Lock bonusLock;
    [SerializeField] private Lock[] obstacleLocks;
    [SerializeField] private Lock[] objectifLocks;
    [SerializeField] private Lock weaponLock;



    readonly string normalDiffText = "Reach wave 10 in any mode (except Drill) to unlock normal difficulty";
    readonly string hardDiffText = "Reach wave 10 in normal difficulty in any mode (except Drill) to unlock hard difficulty";
    
    readonly string rainWheatherText = "Reach wave 5 in objectif option to unlock rain wheather";
    readonly string fogWheatherText = "Reach wave 5 in rain wheather to unlock fog wheather";


    private void Start()
    {
        dataManager = DataManager.InstanceDataManager;
        GetProgressionData();

        ApplyProgressionData();
    }


    private void GetProgressionData()
    {
        progressionData = dataManager.progressionData;
    }
    private void SetProgressionData()
    {
        dataManager.progressionData = progressionData;
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
        GetLocksFromDropdown(ref wheatherLocks, wheatherDropdown);

        if (difficultyLocks.Length > 2)
        {
            difficultyLocks[0].Locked = false;
            difficultyLocks[1].Locked = progressionData.normalDiff; difficultyLocks[1].text.text = normalDiffText;
            difficultyLocks[2].Locked = progressionData.hardDiff; difficultyLocks[2].text.text = hardDiffText;
        }
        
        if (wheatherLocks.Length > 2)
        {
            wheatherLocks[0].Locked = false;
            wheatherLocks[1].Locked = progressionData.rainWheather; wheatherLocks[1].text.text = rainWheatherText;
            wheatherLocks[2].Locked = progressionData.fogWheather; wheatherLocks[2].text.text = fogWheatherText;
        }
    }
}
