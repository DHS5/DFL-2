using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public struct ProgressionData
{
    public bool teamMode;
    public bool zombieMode;

    public bool normalDiff;
    public bool hardDiff;

    public bool rainWheather;
    public bool fogWheather;

    public bool bonusOpt;
    public bool obstacleOpt;
    public bool objectifOpt;
    public bool weaponOpt;
}

public class ProgressionManager : MonoBehaviour
{
    private DataManager dataManager;
    
    
    [HideInInspector] public ProgressionData progressionData;


    [SerializeField] private Lock teamLock;
    [SerializeField] private Lock zombieLock;
    [Space]
    [SerializeField] private Lock normalLock;
    [SerializeField] private Lock hardLock;
    [Space]
    [SerializeField] private Lock rainLock;
    [SerializeField] private Lock fogLock;
    [Space]
    [SerializeField] private Lock bonusLock;
    [SerializeField] private Lock[] obstacleLocks;
    [SerializeField] private Lock[] objectifLocks;
    [SerializeField] private Lock weaponLock;


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

    private void ApplyProgressionData()
    {
        teamLock.Locked = progressionData.teamMode;
        zombieLock.Locked = progressionData.zombieMode;

        normalLock.Locked = progressionData.normalDiff;
        hardLock.Locked = progressionData.hardDiff;

        rainLock.Locked = progressionData.rainWheather;
        fogLock.Locked = progressionData.fogWheather;

        bonusLock.Locked = progressionData.bonusOpt;
        foreach (Lock l in obstacleLocks)
            l.Locked = progressionData.obstacleOpt;
        foreach (Lock l in objectifLocks)
            l.Locked = progressionData.objectifOpt;
        weaponLock.Locked = progressionData.weaponOpt;
    }
}
