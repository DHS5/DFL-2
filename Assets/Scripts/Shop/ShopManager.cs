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
    [SerializeField] private AttackerShopCard[] attackerShopCards;
    [SerializeField] private StadiumShopCard stadiumShopCard;
    [SerializeField] private WeaponShopCard weaponShopCard;
    
    [Header("Shop Screens")]
    [SerializeField] private GameObject[] shopScreens;
    [SerializeField] private GameObject[] shopButtonsBackgrounds;

    [Header("UI prefabs")]
    [SerializeField] private GameObject playerShopButtonPrefab;
    [SerializeField] private GameObject attackerShopButtonPrefab;
    [SerializeField] private GameObject stadiumShopButtonPrefab;
    [SerializeField] private GameObject weaponShopButtonPrefab;


    [Header("Player Shop")]
    public LockerRoom lockerRoom;

    [Header("Team Shop")]
    [SerializeField] private TMP_Dropdown attackerPositionDropdown;

    [Header("Stadium Shop")]
    [Header("Weapon Shop")]



    private bool cardsGenerated = false;

    // ### Properties ###
    public int AttackerPosition
    {
        get { return attackerPositionDropdown.value; }
        set
        {
            CloseTeamContainers();
            teamShopBContainers[value].SetActive(true);
            attackerShopCards[value].gameObject.SetActive(true);
        }
    }
    

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
            GenerateShopButton(cardsContainer.playerCards, playerShopBContainer, playerShopCard, playerShopButtonPrefab, true);
            // Stadium shop buttons
            GenerateShopButton(cardsContainer.stadiumCards, stadiumShopBContainer, stadiumShopCard, stadiumShopButtonPrefab, false);
            // Team shop buttons
            for (int i = 0; i < teamShopBContainers.Length; i++)
                GenerateShopButton(cardsContainer.teamCards.GetCardsByIndex(i), teamShopBContainers[i], attackerShopCards[i], attackerShopButtonPrefab, false);
            AttackerPosition = 0;
            // Weapon shop buttons
            GenerateShopButton(cardsContainer.weaponCards, weaponShopBContainer, weaponShopCard, weaponShopButtonPrefab, false);

            cardsGenerated = true;
        }
    }

    private void GenerateShopButton<T>(List<T> cards, GameObject container, ShopCard shopCard, GameObject shopButtonPrefab, bool isPlayer) where T : ShopCardSO
    {
        bool first = true;
        foreach (T card in cards)
        {
            ShopButton sb = Instantiate(shopButtonPrefab, container.transform).GetComponent<ShopButton>();
            if (!isPlayer) sb.GetCard(card, shopCard, !main.InventoryManager.IsInInventory(card.cardObject));
            else sb.GetCard(card, shopCard, !main.InventoryManager.IsInInventory(card.cardObject), lockerRoom);

            if (first)
            {
                sb.ApplyOnShopCard();
                first = false;
            }
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


    // ### Tools ###

    private void CloseTeamContainers()
    {
        foreach (GameObject g in teamShopBContainers)
            g.SetActive(false);
        foreach (AttackerShopCard sc in attackerShopCards)
            sc.gameObject.SetActive(false);
    }

    public void SetShopScreenActive(int screenNumber)
    {
        foreach (GameObject g in shopScreens) g.SetActive(false);
        foreach (GameObject g in shopButtonsBackgrounds) g.SetActive(false);

        shopScreens[screenNumber].SetActive(true);
        shopButtonsBackgrounds[screenNumber].SetActive(true);
    }
}
