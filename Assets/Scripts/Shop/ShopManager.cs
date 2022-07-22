using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private DataManager dataManager;
    private InventoryManager inventoryManager;

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

    private void Start()
    {
        GetManagers();

        ActuCoinsTexts();
    }


    public void GetManagers()
    {
        dataManager = DataManager.InstanceDataManager;
        inventoryManager = FindObjectOfType<InventoryManager>();
    }



    // ### SHOP ###

    public void GenerateShopButtons()
    {
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

    private void GenerateShopButton(List<CardSO> cards, GameObject container, GameObject shopCPrefab)
    {
        foreach (CardSO card in cards)
        {
            if (!inventoryManager.IsInInventory(card.type.GetObject()))
            {
                ShopButton sb = Instantiate(shopButtonPrefab, container.transform).GetComponent<ShopButton>();
                sb.shopCardPrefab = shopCPrefab;
                sb.cardSO = card;
                sb.GetManagers(in inventoryManager, this);

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


    // ### COINS ###

    private void ActuCoinsTexts()
    {
        foreach (TextMeshProUGUI t in coinsTexts)
            t.text = dataManager.inventoryData.coins.ToString();
    }

    public int GameCoins(GameData data, int score, int wave)
    {
        int coins = 0;

        if (data.gameMode != GameMode.DRILL)
        {
            coins = score * ((int)data.gameDifficulty + 1) * ((int)data.gameWheather + 1) + 100 * (wave * (wave + 1))/2;

            if (data.gameOptions.Contains(GameOption.BONUS) || data.gameOptions.Contains(GameOption.WEAPONS))
                coins /= 3;
            if (data.gameOptions.Contains(GameOption.OBSTACLE))
                coins = (int)(coins * 1.5f);
            if (data.gameOptions.Contains(GameOption.OBJECTIF))
                coins = (int)(coins * 1.5f);
        }
        else if (data.gameDrill == GameDrill.OBJECTIF)
            coins = score / (10 - (int) data.gameDifficulty - (int) data.gameWheather);
        else if (data.gameDrill == GameDrill.ONEVONE)
            coins = score / (10 - (int) data.gameWheather);

        dataManager.inventoryData.coins += coins;
        ActuCoinsTexts();

        return coins;
    }

    public void Buy(int price)
    {
        dataManager.inventoryData.coins -= price;
        ActuCoinsTexts();
    }

    public int WinCoins(GameData data)
    {
        return 1000;
    }
}
