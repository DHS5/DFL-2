using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StadiumCard : ImageCard
{
    public StadiumCardSO stadiumCardSO { get { return cardSO as StadiumCardSO; } }

    [SerializeField] private TextMeshProUGUI percentageText;


    public void ApplyStadiumInfos(StadiumCardSO card)
    {
        cardSO = card;

        image.sprite = stadiumCardSO.mainSprite;
        percentageText.text = (stadiumCardSO.coinsPercentage - 1) * 100 + "%";
        titleText.text = stadiumCardSO.Title;
    }
}
