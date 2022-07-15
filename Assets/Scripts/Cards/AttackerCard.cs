using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class AttackerCard : Card
{
    [Header("Enemy card's specifics")]

    [Tooltip("")]
    [SerializeField] private TextMeshProUGUI positionText;

    [Tooltip("")]
    [SerializeField] private Slider speedSlider;

    [Tooltip("")]
    [SerializeField] private Slider proximitySlider;

    [Tooltip("")]
    [SerializeField] private Slider reactivitySlider;


    protected override void Start()
    {
        base.Start();

        Attacker a = cardSO.prefab.GetComponent<Attacker>();
        positionText.text += cardSO.position;
        speedSlider.value = (a.back2PlayerSpeed + a.defenseSpeed) / 2;
        proximitySlider.value = a.positionRadius;
        reactivitySlider.value = 1 - a.reactivity;
    }
}
