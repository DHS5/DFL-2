using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class AttackerLargeCard : Card
{
    [Header("Attacker card's specifics")]
    [SerializeField] private AttackerCapacityShopCard capacityCard;

    [Header("Locker Room of the Attackers")]
    public AttackerLockerRoom lockerRoom;

    public AttackerCardSO attackerCardSO { get { return cardSO as AttackerCardSO; } }

    public void ApplyCardSOInfo(AttackerCardSO card)
    {
        cardSO = card;

        titleText.text = card.Title;

        AttackerAttributesSO att = card.attribute;

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
