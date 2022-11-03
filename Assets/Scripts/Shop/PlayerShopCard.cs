using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShopCard : ShopCard
{
    [Header("Player shop card's specifics")]
    public PlayerCapacityShopCard capacityCard;

    [Header("Locker Room of the Players")]
    public LockerRoom lockerRoom;

    [Header("Player class")]
    [SerializeField] private Image cardBackground;
    [SerializeField] private TextMeshProUGUI classText;
    public PlayerCardsColorsSO cardColors;


    [HideInInspector] public PlayerCardSO playerCardSO;

    public override void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable, bool _enoughMoney)
    {
        base.GenerateCard(_cardSO, _shopButton, _buyable, _enoughMoney);

        playerCardSO = cardSO as PlayerCardSO;

        cardBackground.color = cardColors.colors[(int)playerCardSO.playerClass];
        classText.color = cardColors.colors[(int)playerCardSO.playerClass];
        classText.text = playerCardSO.playerClass.ToString();

        capacityCard.ApplyInfos(playerCardSO.playerInfo.attributes);

        lockerRoom.ApplyPlayerInfo(playerCardSO);
    }

    public override void RefreshCard()
    {
        capacityCard.ApplyInfos();

        lockerRoom.ApplyPlayerInfo(playerCardSO);
    }

    private void OnEnable()
    {
        lockerRoom.ApplyPlayerInfo(playerCardSO);
    }
}
