using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParkourCard : ImageCard
{
    [Header("Parkour card's specifics")]

    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI rewardText;

    private Slider difficultyGauge;

    private int index;


    public ParkourCardSO parkourCardSO { get { return cardSO as ParkourCardSO; } }


    protected void Awake()
    {
        difficultyGauge = GetComponentInChildren<Slider>();

        difficultyGauge.value = parkourCardSO.difficulty;
    }

    protected override void Start() 
    {
        image.sprite = cardSO.mainSprite;
        rewardText.text = ParkourManager.Won((int)parkourCardSO.parkour) ? parkourCardSO.baseReward.ToString() : parkourCardSO.reward.ToString();
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
