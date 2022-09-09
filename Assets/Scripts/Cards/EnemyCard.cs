using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyCard : Card
{
    [Header("Enemy card's specifics")]
    public EnemyCapacityCard capacityCard;

    [SerializeField] private TextMeshProUGUI positionText;

    public EnemyCardSO enemyCardSO { get { return cardSO as EnemyCardSO; } }

    public override bool InfoActive { get => false; set => infoToggle = null; }

    protected override void Start()
    {
        base.Start();

        positionText.text = enemyCardSO.position;

        DefenderAttributesSO e = enemyCardSO.attribute;

        capacityCard.info.speedInfo.value = e.speed;
        capacityCard.info.accInfo.value = e.acceleration;
        capacityCard.info.rotSpeedInfo.value = e.rotationSpeed;

        capacityCard.info.reactivityInfo.value = 1 - e.reactivity;
        capacityCard.info.intelligenceInfo.value = e.intelligence;
        capacityCard.info.attackDistInfo.value = e.attackDist;

        capacityCard.ApplyInfos();
    }
}
