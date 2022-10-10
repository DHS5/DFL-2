using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StadiumCard : ImageCard
{
    public StadiumCardSO stadiumCardSO { get { return cardSO as StadiumCardSO; } }

    [SerializeField] private TextMeshProUGUI percentageText;

    protected override void Start()
    {
        base.Start();

        image.sprite = stadiumCardSO.shopSprite;
        percentageText.text = (stadiumCardSO.coinsPercentage - 1) * 100 + "%";
    }
}
