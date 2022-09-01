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

        AttackerAttributesSO att = attackerCardSO.attribute;
        positionText.text += attackerCardSO.Position;
        speedSlider.value = (att.back2PlayerSpeed + att.defenseSpeed) / 2;
        proximitySlider.value = att.positionRadius;
        reactivitySlider.value = 1 - att.reactivity;
    }
}
