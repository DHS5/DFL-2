using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class PlayerCard : Card
{
    [Header("Player card's specifics")]
    public PlayerCapacityShopCard capacityCard;

    [Header("Locker room of the players")]
    public LockerRoom lockerRoom;

    [Header("Player class")]
    [SerializeField] private Image cardBackground;
    [SerializeField] private TextMeshProUGUI classText;
    public PlayerCardsColorsSO cardColors;


    public PlayerCardSO playerCardSO { get { return cardSO as PlayerCardSO; } }

    public void ApplyPlayerInfos(PlayerCardSO card)
    {
        cardSO = card;

        titleText.text = card.Title;

        cardBackground.color = cardColors.colors[(int)playerCardSO.playerClass];
        classText.color = cardColors.colors[(int)playerCardSO.playerClass];
        classText.text = playerCardSO.playerClass.ToString();

        capacityCard.ApplyInfos(playerCardSO.playerInfo.attributes);

        lockerRoom.ApplyPlayerInfo(card);
    }

    private void OnEnable()
    {
        lockerRoom.ApplyPlayerInfo(playerCardSO);
    }
}
