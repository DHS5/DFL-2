using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopCardSO : InventoryCardSO
{
    [Header("Shop card specifics")]
    public int price;
    [Space]
    public Sprite shopSprite;
}
