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

        capacityCard.ApplyInfos(att);

        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }

    private void OnEnable()
    {
        lockerRoom.ApplyAttackerInfo(attackerCardSO);
    }
}
