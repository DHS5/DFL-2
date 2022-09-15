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

    public AttackerCardSO attackerCardSO { get { return cardSO as AttackerCardSO; } }

    public override bool InfoActive { get => false; set => infoToggle = null; }


    protected override void Start()
    {
        base.Start();

        image.sprite = attackerCardSO.largeSprite;

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
