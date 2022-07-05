using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourCard : TerrainCard
{
    [Header("Parkour card's specifics")]

    [SerializeField] private Toggle toggle;

    private Slider difficultyGauge;
    
    [SerializeField] private int difficulty;

    private int index;


    protected override void Awake()
    {
        base.Awake();

        difficultyGauge = GetComponentInChildren<Slider>();

        difficultyGauge.value = difficulty;
    }

    public void On()
    {
        toggle.isOn = true;
    }
    public void SetData()
    {
        DataManager.InstanceDataManager.playerPrefs.parkourIndex = index;
        DataManager.InstanceDataManager.gameData.parkour = prefab;
    }
    public void GetIndex(int _index)
    {
        index = _index;
    }
}
