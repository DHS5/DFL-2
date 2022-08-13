using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AttackerShopCard : ShopCard
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


    [HideInInspector] public AttackerCardSO attackerCardSO;

    public override void GenerateCard(ShopCardSO _cardSO, InventoryManager _inventoryManager, ShopManager _shopManager)
    {
        base.GenerateCard(_cardSO, _inventoryManager, _shopManager);

        attackerCardSO = cardSO as AttackerCardSO;

        Attacker a = attackerCardSO.prefab.GetComponent<Attacker>();
        positionText.text += attackerCardSO.position;
        speedSlider.value = (a.back2PlayerSpeed + a.defenseSpeed) / 2;
        proximitySlider.value = a.positionRadius;
        reactivitySlider.value = 1 - a.reactivity;
    }
}
