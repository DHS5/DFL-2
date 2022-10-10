using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class AttackerLargeCard : Card
{
    [Header("Attacker card's specifics")]
    [SerializeField] private AttackerCapacityCard capacityCard;

    [Header("Locker Room of the Attackers")]
    public AttackerLockerRoom lockerRoom;

    public AttackerCardSO attackerCardSO { get { return cardSO as AttackerCardSO; } }

    public void ApplyCardSOInfo(AttackerCardSO card)
    {
        cardSO = card;

        titleText.text = card.Title;

        AttackerAttributesSO att = card.attribute;
        capacityCard.info.position = "Position : " + attackerCardSO.Position;
        capacityCard.info.speedInfo.value = (att.back2PlayerSpeed + att.defenseSpeed) / 2;
        capacityCard.info.rotSpeedInfo.value = att.defenseRotSpeed;
        capacityCard.info.proximityInfo.value = att.positionRadius;
        capacityCard.info.reactivityInfo.value = 1 - att.reactivity;

        capacityCard.ApplyInfos();

        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }

    private void OnEnable()
    {
        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }
}
