using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkourCard : TerrainCard
{
    [Header("Parkour card's specifics")]

    [SerializeField] private Toggle toggle;

    private Slider difficultyGauge;

    private int index;


    protected override void Awake()
    {
        base.Awake();

        difficultyGauge = GetComponentInChildren<Slider>();

        difficultyGauge.value = cardSO.physical;
    }

    protected override void Start() { }

    public void On()
    {
        toggle.isOn = true;
    }
    public void SetData()
    {
        DataManager.InstanceDataManager.playerPrefs.parkourIndex = index;
        DataManager.InstanceDataManager.gameData.parkour = cardSO.prefab;
    }
    public void GetIndex(int _index)
    {
        index = _index;
    }
}
