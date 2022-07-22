using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopCard : ShopCard
{
    [Header("Weapon card's specifics")]

    [Tooltip("")]
    [SerializeField] private Slider rangeSlider;
    
    [Tooltip("")]
    [SerializeField] private Slider angleSlider;

    [Tooltip("")]
    [SerializeField] private Slider ammunitionSlider;

    [Tooltip("")]
    [SerializeField] private Slider reloadTimeSlider;
    
    [Tooltip("")]
    [SerializeField] private Slider maxVictimSlider;

    public override void GenerateCard(CardSO _cardSO, InventoryManager _inventoryManager, ShopManager _shopManager)
    {
        base.GenerateCard(_cardSO, _inventoryManager, _shopManager);

        Weapon w = cardSO.prefab.GetComponent<WeaponBonus>().prefab.GetComponent<Weapon>();
        rangeSlider.value = w.Range;
        angleSlider.value = w.Angle;
        ammunitionSlider.value = w.Ammunition;
        reloadTimeSlider.value = w.ReloadTime;
        maxVictimSlider.value = w.MaxVictim;
    }
}
