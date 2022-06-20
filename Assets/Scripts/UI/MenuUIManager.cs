using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private DataManager dataManager;

    [Header("Game Screen")]
    [SerializeField] private Image playerImage;
    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private Image stadiumImage;
    [SerializeField] private Sprite[] stadiumSprites;


    [SerializeField] private GameObject infoButtons;


    [Header("Enemy Choice Screen")]
    [SerializeField] private GameObject enemyCardsObject;
    private EnemyCard[] enemyCards;
    private int enemyCardsIndex;


    // ### Properties ###

    public bool InfoButtonsOn
    {
        set { infoButtons.SetActive(value); }
    }

    public int GameMode
    {
        set { dataManager.gameData.gameMode = (GameMode) value; }
    }
    public int GameDifficulty
    {
        set { dataManager.gameData.gameDifficulty = (GameDifficulty) value; }
    }
    public int GameWheather
    {
        set { dataManager.gameData.gameWheather = (GameWheather) value; }
    }
    public int GameDrill
    {
        set { dataManager.gameData.gameDrill = (GameDrill) value; }
    }

    public int PlayerIndex
    {
        get { return dataManager.playerPrefs.playerIndex; }
        set
        {
            dataManager.playerPrefs.playerIndex = value;
            dataManager.gameData.playerIndex = value;
        }
    }
    public int StadiumIndex
    {
        get { return dataManager.playerPrefs.stadiumIndex; }
        set
        {
            dataManager.playerPrefs.stadiumIndex = value;
            dataManager.gameData.stadiumIndex = value;
        }
    }



    private void Start()
    {
        settingsManager = SettingsManager.InstanceSettingsManager;
        dataManager = DataManager.InstanceDataManager;

        settingsManager.GetManagers();

        ActuData();

        GetCards();
    }


    // ### Functions ###

    private void ActuData()
    {
        InfoButtonsOn = settingsManager.InfoButtonsOn;

        playerImage.sprite = playerSprites[PlayerIndex];
        stadiumImage.sprite = stadiumSprites[StadiumIndex];
    }

    /// <summary>
    /// Removes or adds a game option
    /// </summary>
    /// <param name="b">True --> Add / False --> Remove</param>
    /// <param name="option">Game option to add/remove</param>
    public void ChooseOption(int option)
    {
        if (!dataManager.gameData.gameOptions.Contains((GameOption)option)) { dataManager.gameData.gameOptions.Add((GameOption)option); }
        else { dataManager.gameData.gameOptions.Remove((GameOption)option); }
    }


    public void NextPlayer()
    {
        if (PlayerIndex == playerSprites.Length - 1) { PlayerIndex = 0; }
        else { PlayerIndex++; }

        playerImage.sprite = playerSprites[PlayerIndex];
    }
    public void PrevPlayer()
    {
        if (PlayerIndex == 0) { PlayerIndex = playerSprites.Length - 1; }
        else { PlayerIndex--; }

        playerImage.sprite = playerSprites[PlayerIndex];
    }
    public void NextStadium()
    {
        if (StadiumIndex == stadiumSprites.Length - 1) { StadiumIndex = 0; }
        else { StadiumIndex++; }

        stadiumImage.sprite = stadiumSprites[StadiumIndex];
    }
    public void PrevStadium()
    {
        if (StadiumIndex == 0) { StadiumIndex = stadiumSprites.Length - 1; }
        else { StadiumIndex--; }

        stadiumImage.sprite = stadiumSprites[StadiumIndex];
    }


    private void GetCards()
    {
        enemyCards = enemyCardsObject.GetComponentsInChildren<EnemyCard>();
        foreach (EnemyCard ec in enemyCards)
            ec.gameObject.SetActive(false);
        enemyCards[enemyCardsIndex].gameObject.SetActive(true);
    }

    private void NextCard(Card[] cards, ref int index)
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        if (index == cards.Length - 1) { index = 0; }
        else { index++; }

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;
        dataManager.gameData.enemy = cards[index].prefab;
    }
    private void PrevCard(Card[] cards, ref int index)
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        if (index == 0) { index = cards.Length - 1; }
        else { index--; }

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;
        dataManager.gameData.enemy = cards[index].prefab;
    }
    public void NextCardEnemy() { NextCard(enemyCards, ref enemyCardsIndex); }
    public void PrevCardEnemy() { PrevCard(enemyCards, ref enemyCardsIndex); }


    // ### Tools ###

    public void SetAsLastSibling(GameObject go)
    {
        go.transform.SetAsLastSibling();
    }
}
