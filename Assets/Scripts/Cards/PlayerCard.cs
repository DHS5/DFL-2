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

        PlayerAttributesSO p = card.playerInfo.attributes;

        capacityCard.info.speedInfo.value = p.NormalSpeed;
        capacityCard.info.sideSpeedInfo.value = p.NormalSideSpeed;
        capacityCard.info.sprintInfo.value = p.AccelerationM;
        capacityCard.info.staminaInfo.value = p.accelerationTime / p.accelerationRestTime;
        capacityCard.info.slowInfo.value = p.SlowM;
        capacityCard.info.shiftInfo.value = p.SlowSideSpeed;
        capacityCard.info.jumpHeightInfo.value = p.JumpHeight;
        capacityCard.info.jumpStaminaInfo.value = p.JumpStamina / p.JumpRechargeTime;

        capacityCard.info.dirSensitivityInfo.value = p.DirSensitivity;
        capacityCard.info.dirGravityInfo.value = p.DirGravity;
        capacityCard.info.accSensitivityInfo.value = p.AccSensitivity;
        capacityCard.info.accGravityInfo.value = p.AccGravity;

        capacityCard.info.canJuke = p.CanJuke;
        capacityCard.info.canFeint = p.CanFeint;
        capacityCard.info.canSpin = p.CanSpin;
        capacityCard.info.canJukeSpin = p.CanJukeSpin;
        capacityCard.info.canSlide = p.CanSlide;
        capacityCard.info.canFlip = p.CanFlip;
        capacityCard.info.canTruck = p.CanTruck;
        capacityCard.info.canHighKnee = p.CanHighKnee;
        capacityCard.info.canSprintFeint = p.CanSprintFeint;
        capacityCard.info.canHurdle = p.CanHurdle;

        capacityCard.ApplyInfos();

        lockerRoom.ApplyPlayerInfo(card);
    }

    private void OnEnable()
    {
        lockerRoom.ApplyPlayerInfo(playerCardSO);
    }
}
