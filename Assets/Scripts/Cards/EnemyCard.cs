using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyCard : Card
{
    [Header("Enemy card's specifics")]
    [SerializeField] private TextMeshProUGUI positionText;

    [SerializeField] private Slider speedSlider;

    [SerializeField] private Slider intelligenceSlider;

    [SerializeField] private Slider reactivitySlider;

    public EnemyCardSO enemyCardSO { get { return cardSO as EnemyCardSO; } }

    protected override void Start()
    {
        base.Start();

        positionText.text = "position :\n" + enemyCardSO.position;

        DefenderAttributesSO e = enemyCardSO.attribute;

        speedSlider.value = e.speed;
        intelligenceSlider.value = e.intelligence;
        reactivitySlider.value = 1 - e.reactivity;
    }
}
