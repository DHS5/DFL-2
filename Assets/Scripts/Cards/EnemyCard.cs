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


    protected override void Start()
    {
        base.Start();

        image.sprite = cardSO.sprite100x120;

        speedSlider.value = cardSO.prefab.GetComponent<NavMeshAgent>().speed;

        Enemy e = cardSO.prefab.GetComponent<Enemy>();
        intelligenceSlider.value = e.intelligence;
        reactivitySlider.value = 1 - e.reactivity;
    }
}
