using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyCard : Card
{
    [Header("Manager")]
    [SerializeField] private CardManager cardManager;

    [Header("Enemy card's specifics")]
    public EnemyCapacityCard capacityCard;

    [Header("Locker room of the enemies")]
    public EnemyLockerRoom lockerRoom;

    [SerializeField] private TextMeshProUGUI positionText;

    public EnemyCardSO enemyCardSO { get { return cardSO as EnemyCardSO; } }


    public void ApplyEnemyInfos(EnemyCardSO card)
    {
        cardSO = card;

        titleText.text = card.Title;
        positionText.text = card.position;

        DefenderAttributesSO e = enemyCardSO.attribute;

        capacityCard.info.speedInfo.value = e.speed;
        capacityCard.info.accInfo.value = e.acceleration;
        capacityCard.info.rotSpeedInfo.value = e.rotationSpeed;

        capacityCard.info.reactivityInfo.value = 1 - e.reactivity;
        capacityCard.info.intelligenceInfo.value = e.intelligence;
        capacityCard.info.attackDistInfo.value = e.attackDist;

        capacityCard.ApplyInfos();

        lockerRoom.ApplyEnemyInfo(card);
    }

    private void OnEnable()
    {
        cardManager.ActuEnemy();
    }
}
