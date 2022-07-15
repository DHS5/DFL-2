using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private DataManager dataManager;

    [Header("Cards scriptable object")]
    [SerializeField] private CardsContainerSO cardsContainer;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI[] coinsTexts;
    [Space]
    [SerializeField] private GameObject shopBackground;
    [SerializeField] private GameObject shopCardContainer;
    [Space]
    [SerializeField] private GameObject playerShopCContainer;
    [SerializeField] private GameObject stadiumShopCContainer;
    [SerializeField] private GameObject teamShopCContainer;
    [SerializeField] private GameObject weaponShopCContainer;

    [Header("UI prefabs")]
    [SerializeField] private GameObject shopButtonPrefab;
    [SerializeField] private GameObject playerShopCPrefab;
    [SerializeField] private GameObject stadiumShopCPrefab;
    [SerializeField] private GameObject attackerShopCPrefab;
    [SerializeField] private GameObject weaponShopCPrefab;




    // ### Functions ###

    private void Start()
    {
        dataManager = DataManager.InstanceDataManager;

        ActuCoinsTexts();

        GenerateShopButtons();
    }



    // ### SHOP ###

    private void GenerateShopButtons()
    {
        // Player shop buttons
        GenerateShopButton(cardsContainer.playerCards, playerShopCContainer, playerShopCPrefab);
    }

    private void GenerateShopButton(List<CardSO> cards, GameObject container, GameObject shopCPrefab)
    {
        foreach (CardSO card in cards)
        {
            ShopButton sb = Instantiate(shopButtonPrefab, container.transform).GetComponent<ShopButton>();
            sb.shopCardPrefab = shopCPrefab;
            sb.cardSO = card;
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


    // ### COINS ###

    private void ActuCoinsTexts()
    {
        foreach (TextMeshProUGUI t in coinsTexts)
            t.text = dataManager.progressData.coins.ToString();
    }

    public void GameCoins(GameData data, int score)
    {
        int coins = 0;

        if (data.gameMode != GameMode.DRILL)
        {
            coins = score * ((int)data.gameDifficulty + 1) * ((int)data.gameWheather + 1);

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

        dataManager.progressData.coins += coins;
        ActuCoinsTexts();
    }

    public int WinCoins(GameData data)
    {
        return 1000;
    }
}
