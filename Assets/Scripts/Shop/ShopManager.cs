using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private MenuMainManager main;

    [Header("Cards scriptable object")]
    [SerializeField] private CardsContainerSO cardsContainer;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI[] coinsTexts;
    [Space]
    [SerializeField] private GameObject shopBackground;
    [SerializeField] private GameObject shopCardContainer;
    [Space]
    [SerializeField] private GameObject playerShopBContainer;
    [SerializeField] private GameObject stadiumShopBContainer;
    [SerializeField] private GameObject teamShopBContainer;
    [SerializeField] private GameObject weaponShopBContainer;

    [Header("UI prefabs")]
    [SerializeField] private GameObject shopButtonPrefab;
    [SerializeField] private GameObject playerShopCPrefab;
    [SerializeField] private GameObject stadiumShopCPrefab;
    [SerializeField] private GameObject attackerShopCPrefab;
    [SerializeField] private GameObject weaponShopCPrefab;

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
            GenerateShopButton(cardsContainer.playerCards, playerShopBContainer, playerShopCPrefab);
            // Stadium shop buttons
            GenerateShopButton(cardsContainer.stadiumCards, stadiumShopBContainer, stadiumShopCPrefab);
            // Team shop buttons
            GenerateShopButton(cardsContainer.teamCards, teamShopBContainer, attackerShopCPrefab);
            // Weapon shop buttons
            GenerateShopButton(cardsContainer.weaponCards, weaponShopBContainer, weaponShopCPrefab);

            cardsGenerated = true;
        }
    }

    private void GenerateShopButton<T>(List<T> cards, GameObject container, GameObject shopCPrefab) where T : ShopCardSO
    {
        foreach (T card in cards)
        {
            if (!main.InventoryManager.IsInInventory(card.cardObject))
            {
                ShopButton sb = Instantiate(shopButtonPrefab, container.transform).GetComponent<ShopButton>();
                sb.shopCardPrefab = shopCPrefab;
                sb.cardSO = card;
                sb.GetManagers(main.InventoryManager, this);

                shopButtons.Add(sb);
            }
        }
    }

    public GameObject OpenShop()
    {
        shopBackground.SetActive(true);

        return shopCardContainer;
    }

    public void DeactivateShopCards()
    {
        shopBackground.SetActive(false);

        foreach(ShopCard sc in shopCardContainer.GetComponentsInChildren<ShopCard>())
            sc.gameObject.SetActive(false);
    }

    public void DestroyShopButton(CardSO card)
    {
        foreach (ShopButton sb in shopButtons)
        {
            if (sb.cardSO == card)
                Destroy(sb.gameObject);
        }
    }

    public void Buy(int price)
    {
        main.DataManager.inventoryData.coins -= price;
        ActuCoinsTexts();
    }


    // ### COINS ###

    private void ActuCoinsTexts()
    {
        foreach (TextMeshProUGUI t in coinsTexts)
            t.text = main.DataManager.inventoryData.coins.ToString();
    }
}
