using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private Image picture;


    private ShopCardSO cardSO;

    private ShopCard shopCard;

    [HideInInspector] public bool buyable;


    public void GetCard(ShopCardSO _cardSO, ShopCard _shopCard, bool _buyable)
    {
        cardSO = _cardSO;
        shopCard = _shopCard;
        buyable = _buyable;
        picture.sprite = cardSO.mainSprite;
    }


    /// <summary>
    /// Gives the ShopCard the cardSO and generates it
    /// </summary>
    /// <param name="g">Parent of the shop card</param>
    public void ApplyOnShopCard()
    {
        if (shopCard != null)
        {
            shopCard.GenerateCard(cardSO, this, buyable);
        }
    }
}
