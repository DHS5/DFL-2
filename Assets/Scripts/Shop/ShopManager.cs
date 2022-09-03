using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private MenuMainManager main;

    [Header("Cards scriptable object")]
    [SerializeField] private CardsContainerSO cardsContainer;

    [Header("Coins texts")]
    [SerializeField] private TextMeshProUGUI[] coinsTexts;

    [Header("Shop Button Containers")]
    [SerializeField] private GameObject playerShopBContainer;
    [SerializeField] private GameObject stadiumShopBContainer;
    [SerializeField] private GameObject[] teamShopBContainers;
    [SerializeField] private GameObject weaponShopBContainer;

    [Header("Shop Cards")]
    [SerializeField] private PlayerShopCard playerShopCard;
    [SerializeField] private AttackerShopCard attackerShopCard;
    [SerializeField] private StadiumShopCard stadiumShopCard;
    [SerializeField] private WeaponShopCard weaponShopCard;

    [Header("UI prefabs")]
    [SerializeField] private GameObject characterShopButtonPrefab;
    [SerializeField] private GameObject simpleShopButtonPrefab;

    private List<ShopButton> shopButtons = new(); 


    private bool cardsGenerated = false;

    // ### Functions ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }



    // ### SHOP ###

    public void GenerateShopButtons()
    {
        ActuCoinsTexts();

        if (!cardsGenerated)
        {
            // Player shop buttons
            GenerateShopButton(cardsContainer.playerCards, playerShopBContainer, playerShopCard, characterShopButtonPrefab);
            // Stadium shop buttons
            GenerateShopButton(cardsContainer.stadiumCards, stadiumShopBContainer, stadiumShopCard, simpleShopButtonPrefab);
            // Team shop buttons
            for (int i = 0; i < teamShopBContainers.Length; i++)
                GenerateShopButton(cardsContainer.teamCards.GetCardsByIndex(i), teamShopBContainers[i], attackerShopCard, characterShopButtonPrefab);
            // Weapon shop buttons
            GenerateShopButton(cardsContainer.weaponCards, weaponShopBContainer, weaponShopCard, simpleShopButtonPrefab);

            cardsGenerated = true;
        }
    }

    private void GenerateShopButton<T>(List<T> cards, GameObject container, ShopCard shopCard, GameObject shopButtonPrefab) where T : ShopCardSO
    {
        foreach (T card in cards)
        {            
            ShopButton sb = Instantiate(shopButtonPrefab, container.transform).GetComponent<ShopButton>();
            sb.GetCard(card, shopCard, !main.InventoryManager.IsInInventory(card.cardObject));

            shopButtons.Add(sb);
        }
    }


    public void ActuShopButton(ShopButton shopButton, bool _buyable)
    {
        shopButton.buyable = _buyable;
    }

    public void Buy(ShopCard card)
    {
        if (main.DataManager.inventoryData.coins >= card.cardSO.price)
        {
            main.DataManager.inventoryData.coins -= card.cardSO.price;
            ActuCoinsTexts();

            main.InventoryManager.AddToInventory(card.cardSO.cardObject);
            ActuShopButton(card.shopButton, false);
            card.Buyable = false;
        }
        else
        {
            Debug.Log("You don't have enough coins to buy " + card.cardSO.Title);
        }
    }


    // ### COINS ###

    private void ActuCoinsTexts()
    {
        foreach (TextMeshProUGUI t in coinsTexts)
            t.text = main.DataManager.inventoryData.coins.ToString();
    }
}
