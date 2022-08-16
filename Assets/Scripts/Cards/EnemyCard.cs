using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyCard : Card
{
    [Header("Enemy card's specifics")]
    [Tooltip("")]
    [SerializeField] private Slider speedSlider;

    [Tooltip("")]
    [SerializeField] private Slider intelligenceSlider;

    [Tooltip("")]
    [SerializeField] private Slider reactivitySlider;

    public EnemyCardSO enemyCardSO { get { return cardSO as EnemyCardSO; } }

    protected override void Start()
    {
        base.Start();

        speedSlider.value = enemyCardSO.attribute.speed;

        DefenderAttributesSO e = enemyCardSO.attribute;
        intelligenceSlider.value = e.intelligence;
        reactivitySlider.value = 1 - e.reactivity;
    }
}
