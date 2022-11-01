using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParkourCard : ImageCard
{
    [Header("Parkour card's specifics")]

    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI firstRewardText;
    [SerializeField] private TextMeshProUGUI thenText;

    [SerializeField] private Slider difficultyGauge;

    [Space]
    [SerializeField] private GameObject lockGO;


    public ParkourCardSO parkourCardSO { get { return cardSO as ParkourCardSO; } }


    protected override void Start()
    {
        titleText.text = parkourCardSO.Title;
        image.sprite = cardSO.mainSprite;
        firstRewardText.text = parkourCardSO.Reward.ToString();
        thenText.text = parkourCardSO.BaseReward.ToString();
        difficultyGauge.value = parkourCardSO.Difficulty;

        if (parkourCardSO.locked) Lock();
    }

    public void On()
    {
        toggle.isOn = true;
    }
    public void SetData()
    {
        parkourCardSO.SetActive();
    }

    public void GetToggleGroup(ToggleGroup group)
    {
        toggle.group = group;
    }

    public void Lock()
    {
        toggle.interactable = false;
        lockGO.SetActive(true);
    }
}
