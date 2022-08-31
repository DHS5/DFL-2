using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopCard : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private ShopManager shopManager;

    [Header("UI components")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI popupText;

    protected ShopCardSO cardSO;
    protected int price;


    public virtual void GenerateCard(ShopCardSO _cardSO, InventoryManager _inventoryManager, ShopManager _shopManager)
    {
        cardSO = _cardSO;
        inventoryManager = _inventoryManager;
        shopManager = _shopManager;

        text.text = cardSO.Title;
        image.sprite = cardSO.mainSprite;
        price = cardSO.price;
        buttonText.text = price.ToString();
        popupText.text = "Are you sure you want to buy " + cardSO.Title + " ?";
    }

    public virtual void Buy()
    {
        DataManager dataManager = DataManager.InstanceDataManager;
        if (dataManager != null)
        {
            if (dataManager.inventoryData.coins >= price)
            {
                Debug.Log("Buy");
                shopManager.Buy(cardSO.price);
                inventoryManager.AddToInventory(cardSO.cardObject);
                shopManager.DestroyShopButton(cardSO);
                shopManager.DeactivateShopCards();
            }
            else
            {
                Debug.Log("You don't have enough coins to buy " + text.text);
            }
        }
    }
}
