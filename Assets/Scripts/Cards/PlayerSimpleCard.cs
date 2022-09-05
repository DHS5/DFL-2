using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerSimpleCard : Card
{
    [Header("Player card's specifics")]
    public PlayerCapacityCard capacityCard;


    public PlayerCardSO playerCardSO { get { return cardSO as PlayerCardSO; } }

    protected override void Start()
    {
        base.Start();

        capacityCard.info.physicalInfo.value = playerCardSO.physical;
        capacityCard.info.handlingInfo.value = playerCardSO.handling;
        capacityCard.info.skillsInfo.value = playerCardSO.skills;

        capacityCard.ApplyInfos();
    }
}
