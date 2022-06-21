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


    protected override void Start()
    {
        base.Start();

        Player p = prefab.GetComponent<Player>();

        physicalSlider.value = p.physical;
        handlingSlider.value = p.handling;
        skillsSlider.value = p.skills;
    }
}
