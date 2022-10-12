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

        capacityCard.ApplyInfos(att);

        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }

    public override void RefreshCard()
    {
        capacityCard.ApplyInfos(attackerCardSO.attribute);

        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }


    private void OnEnable()
    {
        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }
}
