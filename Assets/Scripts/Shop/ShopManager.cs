using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private DataManager dataManager;


    [SerializeField] private TextMeshProUGUI[] coinsTexts;

    [SerializeField] private GameObject shopBackground;
    [SerializeField] private GameObject shopCardContainer;




    // ### Functions ###

    private void Start()
    {
        dataManager = DataManager.InstanceDataManager;

        ActuCoinsTexts();
    }



    // ### SHOP ###

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
