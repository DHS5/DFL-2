using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    private ShopManager shopManager;
    private InventoryManager inventoryManager;


    [Header("UI components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Fields")]
    [Tooltip("Text to display under the button")]
    [SerializeField] private string title;
    [Tooltip("Sprite to display on the button")]
    [SerializeField] private Sprite sprite;
    [Tooltip("Sprite to display on the shop card")]
    [SerializeField] private Sprite shopCardSprite;
    [Tooltip("Shop card prefab")]
    [HideInInspector] public GameObject shopCardPrefab;
    [Tooltip("Shop object prefab (object to buy)")]
    [SerializeField] private GameObject shopObjectPrefab;
    [Tooltip("Price of the object")]
    [SerializeField] private int price;


    [HideInInspector] public CardSO cardSO;


    private ShopCard shopCard;


    private void Start()
    {
        text.text = cardSO.Title;
        button.image.sprite = cardSO.shopSprite;
    }


    /// <summary>
    /// Instantiate a shop card and passes it the corresponding prefab
    /// </summary>
    /// <param name="g">Parent of the shop card</param>
    public void InstantiateShopCard()
    {
        if (shopCard == null)
        {
            shopCard = Instantiate(shopCardPrefab, shopManager.OpenShop().transform).GetComponent<ShopCard>();
            shopCard.GenerateCard(cardSO, inventoryManager, shopManager);
        }
        else
        {
            shopCard.gameObject.SetActive(true);
            shopManager.OpenShop();
        }
    }

    public void GetManagers(in InventoryManager inv, in ShopManager shop)
    {
        inventoryManager = inv;
        shopManager = shop;
    }
}
