using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class AttackerCard : Card
{
    [Header("Attacker card's specifics")]

    [Tooltip("")]
    [SerializeField] private TextMeshProUGUI positionText;

    [Tooltip("")]
    [SerializeField] private Slider speedSlider;

    [Tooltip("")]
    [SerializeField] private Slider proximitySlider;

    [Tooltip("")]
    [SerializeField] private Slider reactivitySlider;

    public AttackerCardSO attackerCardSO { get { return cardSO as AttackerCardSO; } }

    protected override void Start()
    {
        base.Start();

        AttackerAttributesSO att = attackerCardSO.attribute;
        positionText.text += attackerCardSO.Position;
        speedSlider.value = (att.back2PlayerSpeed + att.defenseSpeed) / 2;
        proximitySlider.value = att.positionRadius;
        reactivitySlider.value = 1 - att.reactivity;
    }
}
