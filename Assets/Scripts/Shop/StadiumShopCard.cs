using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StadiumShopCard : ShopCard
{
    [Header("Stadium's card specifics")]
    [SerializeField] private TextMeshProUGUI percentageText;

    public StadiumCardSO stadiumCardSO { get { return cardSO as StadiumCardSO; } }

    public override void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable)
    {
        base.GenerateCard(_cardSO, _shopButton, _buyable);

        percentageText.text = "+" + (stadiumCardSO.coinsPercentage - 1) * 100 + "%";
    }
}
