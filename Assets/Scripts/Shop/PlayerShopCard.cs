using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShopCard : ShopCard
{
    [Header("Player shop card's specifics")]
    public PlayerCapacityShopCard capacityCard;


    [HideInInspector] public PlayerCardSO playerCardSO;

    public override void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable)
    {
        base.GenerateCard(_cardSO, _shopButton, _buyable);

        playerCardSO = cardSO as PlayerCardSO;

        PlayerAttributesSO p = playerCardSO.prefab.GetComponentInChildren<PlayerController>().playerAtt;
        
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
        capacityCard.info.canSlideTackle = false;

        capacityCard.ApplyInfos();
    }
}
