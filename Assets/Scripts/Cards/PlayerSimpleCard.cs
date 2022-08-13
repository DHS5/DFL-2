using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerSimpleCard : Card
{
    [Header("Player card's specifics")]
    [Tooltip("")]
    [SerializeField] private Slider physicalSlider;

    [Tooltip("")]
    [SerializeField] private Slider handlingSlider;

    [Tooltip("")]
    [SerializeField] private Slider skillsSlider;


    [HideInInspector] public PlayerCardSO playerCardSO;

    protected override void Start()
    {
        base.Start();

        playerCardSO = cardSO as PlayerCardSO;

        physicalSlider.value = playerCardSO.physical;
        handlingSlider.value = playerCardSO.handling;
        skillsSlider.value = playerCardSO.skills;
    }
}
