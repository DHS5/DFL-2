using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopCard : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private GameObject lockGO;

    public ShopCardSO cardSO { get; private set; }
    public ShopButton shopButton { get; private set; }
    protected int Price
    {
        set { buttonText.text = value.ToString(); }
    }
    public bool Buyable
    {
        set { buyButton.gameObject.SetActive(value); }
    }

    public virtual void GenerateCard(ShopCardSO _cardSO, ShopButton _shopButton, bool _buyable, bool enoughMoney)
    {
        cardSO = _cardSO;
        shopButton = _shopButton;

        Buyable = _buyable;
        text.text = cardSO.Title;
        if (image != null) image.sprite = cardSO.shopSprite;
        Price = cardSO.price;
        popupText.text = "Are you sure you want to buy " + cardSO.Title + " ?";
        buyButton.interactable = enoughMoney;
        lockGO.SetActive(cardSO.locked);
    }
}
