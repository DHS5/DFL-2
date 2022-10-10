using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AttackerShopCard : ShopCard
{
    [Header("Attacker card's specifics")]
    public AttackerCapacityShopCard capacityCard;

    [Header("Locker Room of the Attackers")]
    public AttackerLockerRoom lockerRoom;


    [HideInInspector] public AttackerCardSO attackerCardSO;

    public override void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable, bool _enoughMoney)
    {
        base.GenerateCard(_cardSO, _shopButton, _buyable, _enoughMoney);

        attackerCardSO = cardSO as AttackerCardSO;

        AttackerAttributesSO att = attackerCardSO.attribute;

        capacityCard.info.speedInfo.value = att.speed;
        capacityCard.info.defSpeedInfo.value = att.defenseSpeed;
        capacityCard.info.repositionSpeedInfo.value = att.back2PlayerSpeed;

        capacityCard.info.rotSpeedInfo.value = att.rotationSpeed;
        capacityCard.info.defRotSpeedInfo.value = att.defenseRotSpeed;

        capacityCard.info.accInfo.value = att.acceleration;
        capacityCard.info.sizeInfo.value = att.size.y;

        capacityCard.info.reactivityInfo.value = 1 - att.reactivity;
        capacityCard.info.proximityInfo.value = att.positionRadius;


        capacityCard.ApplyInfos();

        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }


    private void OnEnable()
    {
        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }
}
