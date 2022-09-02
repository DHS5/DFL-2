using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class AttackerCard : Card
{
    [Header("Attacker card's specifics")]

    [SerializeField] private AttackerCapacityCard capacityCard;

    public AttackerCardSO attackerCardSO { get { return cardSO as AttackerCardSO; } }

    protected override void Start()
    {
        base.Start();

        AttackerAttributesSO att = attackerCardSO.attribute;
        capacityCard.info.position = "Position : " + attackerCardSO.Position;
        capacityCard.info.speedInfo.value = (att.back2PlayerSpeed + att.defenseSpeed) / 2;
        capacityCard.info.rotSpeedInfo.value = att.defenseRotSpeed;
        capacityCard.info.proximityInfo.value = att.positionRadius;
        capacityCard.info.reactivityInfo.value = 1 - att.reactivity;

        capacityCard.ApplyInfos();
    }

    public void ApplyCardSOInfo(AttackerCardSO card)
    {
        InfoActive = false;
        cardSO = card;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            Start();
    }
}
