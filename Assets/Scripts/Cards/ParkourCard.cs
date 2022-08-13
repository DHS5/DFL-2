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


    [HideInInspector] public ParkourCardSO parkourCardSO;


    protected void Awake()
    {
        difficultyGauge = GetComponentInChildren<Slider>();

        difficultyGauge.value = parkourCardSO.difficulty;
    }

    protected override void Start() 
    {
        image.sprite = cardSO.mainSprite; 
    }

    public void On()
    {
        toggle.isOn = true;
    }
    public void SetData()
    {
        DataManager.InstanceDataManager.playerPrefs.parkourIndex = index;
        DataManager.InstanceDataManager.gameData.parkour = parkourCardSO.prefab;
    }
    public void GetIndex(int _index)
    {
        index = _index;
    }
}
