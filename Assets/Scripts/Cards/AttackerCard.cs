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

        Attacker a = attackerCardSO.prefab.GetComponent<Attacker>();
        positionText.text += attackerCardSO.position;
        speedSlider.value = (a.back2PlayerSpeed + a.defenseSpeed) / 2;
        proximitySlider.value = a.positionRadius;
        reactivitySlider.value = 1 - a.reactivity;
    }
}
