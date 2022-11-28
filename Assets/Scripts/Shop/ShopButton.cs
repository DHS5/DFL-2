using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private Image picture;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject selector;


    private ShopCardSO cardSO;

    private ShopCard shopCard;

    [HideInInspector] public bool buyable;
    [HideInInspector] public bool enoughMoney;


    public void GetCard(ShopCardSO _cardSO, ShopCard _shopCard, bool _buyable, bool _enoughMoney)
    {
        cardSO = _cardSO;
        shopCard = _shopCard;
        buyable = _buyable;
        if (picture != null) picture.sprite = cardSO.mainSprite;
        if (nameText != null) nameText.text = cardSO.Title;
        enoughMoney = _enoughMoney;

        DisableSelector();
    }


    /// <summary>
    /// Gives the ShopCard the cardSO and generates it
    /// </summary>
    /// <param name="g">Parent of the shop card</param>
    public void ApplyOnShopCard()
    {
        if (shopCard != null)
        {
            shopCard.GenerateCard(cardSO, this, buyable, enoughMoney);
            selector.SetActive(true);
        }
    }

    public void DisableSelector()
    {
        selector.SetActive(false);
    }
}
