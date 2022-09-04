using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopCard : ShopCard
{
    [Header("Weapon card's specifics")]
    public WeaponCapacityCard capacityCard;

    [HideInInspector] public WeaponCardSO weaponCardSO;

    public override void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable)
    {
        base.GenerateCard(_cardSO, _shopButton, _buyable);

        weaponCardSO = cardSO as WeaponCardSO;

        Weapon w = weaponCardSO.prefab.GetComponent<WeaponBonus>().prefab.GetComponent<Weapon>();

        capacityCard.info.rangeInfo.value = w.Range;
        capacityCard.info.angleInfo.value = w.Angle;
        capacityCard.info.ammunitionInfo.value = w.Ammunition;
        capacityCard.info.reloadInfo.value = w.ReloadTime;
        capacityCard.info.maxVictimInfo.value = w.MaxVictim;

        capacityCard.ApplyInfos();
    }
}
