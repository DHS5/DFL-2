using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private DataManager dataManager;

    private List<GameOption> defenderOptions = new List<GameOption>();
    private List<GameOption> zombieOptions = new List<GameOption>();

    [Header("Game Screen")]
    [SerializeField] private GameObject playerSimpleCardsObject;
    private PlayerSimpleCard[] playerSimpleCards;

    [SerializeField] private Image stadiumImage;
    [SerializeField] private Sprite[] stadiumSprites;


    [SerializeField] private GameObject infoButtons;


    [Header("Player Choice Screen")]
    [SerializeField] private GameObject playerCompCardsObject;
    private PlayerCompleteCard[] playerCompCards;


    [Header("Enemy Choice Screen")]
    [SerializeField] private GameObject enemyCardsObject;
    private EnemyCard[] enemyCards;


    [Header("Team Choice Screen")]
    [SerializeField] private GameObject[] attackerCardsObjects;
    private AttackerCard[][] attackerCards = new AttackerCard[5][];


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
        set { dataManager.playerPrefs.playerIndex = value; }
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
    public int EnemyIndex
    {
        get { return dataManager.playerPrefs.enemyIndex; }
        set { dataManager.playerPrefs.enemyIndex = value; }
    }
    public int[] AttackerIndex
    {
        get { return dataManager.playerPrefs.teamIndex; }
        set { dataManager.playerPrefs.teamIndex = value; }
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

        stadiumImage.sprite = stadiumSprites[StadiumIndex];
    }

    /// <summary>
    /// Removes or adds a game option
    /// </summary>
    /// <param name="b">True --> Add / False --> Remove</param>
    /// <param name="option">Game option to add/remove</param>
    public void ChooseOption(int option)
    {
        List<GameOption> gameOptions = (dataManager.gameData.gameMode == global::GameMode.ZOMBIE) ? zombieOptions : defenderOptions;

        if (!gameOptions.Contains((GameOption)option)) { gameOptions.Add((GameOption)option); }
        else { gameOptions.Remove((GameOption)option); }

        dataManager.gameData.gameOptions = new List<GameOption>(gameOptions);

        foreach (GameOption go in dataManager.gameData.gameOptions)
        {
            Debug.Log(go.ToString());
        }
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



    private void GetCard<T>(out T[] cards, GameObject cardObject, int index, ref GameObject g)
    {
        cards = cardObject.GetComponentsInChildren<T>();
        Card[] cardsBis = cardObject.GetComponentsInChildren<Card>();
        foreach (Card c in cardsBis)
            c.gameObject.SetActive(false);
        cardsBis[index].gameObject.SetActive(true);
        g = cardsBis[index].prefab;
    }

    private void GetCards()
    {
        // Player simple cards
        GetCard(out playerSimpleCards, playerSimpleCardsObject, PlayerIndex, ref dataManager.gameData.player);

        // Player complete cards
        GetCard(out playerCompCards, playerCompCardsObject, PlayerIndex, ref dataManager.gameData.player);

        // Enemy cards
        GetCard(out enemyCards, enemyCardsObject, EnemyIndex, ref dataManager.gameData.enemy);

        // Attacker cards
        for (int i = 0; i < attackerCardsObjects.Length; i++)
            GetCard(out attackerCards[i], attackerCardsObjects[i], AttackerIndex[i], ref dataManager.gameData.team[i]);
    }

    private int NextCard(Card[] cards, int index, ref GameObject g)
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        Next(ref index, cards.Length - 1);

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;
        g = cards[index].prefab;

        return index;
    }
    private int PrevCard(Card[] cards, int index, ref GameObject g)
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        Prev(ref index, cards.Length - 1);

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;
        g = cards[index].prefab;

        return index;
    }


    public void NextCardPlayer() 
    {
        NextCard(playerCompCards, PlayerIndex, ref dataManager.gameData.player);
        PlayerIndex = NextCard(playerSimpleCards, PlayerIndex, ref dataManager.gameData.player); 
    }
    public void PrevCardPlayer() 
    {
        PrevCard(playerCompCards, PlayerIndex, ref dataManager.gameData.player);
        PlayerIndex = PrevCard(playerSimpleCards, PlayerIndex, ref dataManager.gameData.player);
    }
    public void NextCardEnemy() { EnemyIndex = NextCard(enemyCards, EnemyIndex, ref dataManager.gameData.enemy); }
    public void PrevCardEnemy() { EnemyIndex = PrevCard(enemyCards, EnemyIndex, ref dataManager.gameData.enemy); }

    public void NextCardAttacker(int i) { AttackerIndex[i] = NextCard(attackerCards[i], AttackerIndex[i], ref dataManager.gameData.team[i]); }
    public void PrevCardAttacker(int i) { AttackerIndex[i] = PrevCard(attackerCards[i], AttackerIndex[i], ref dataManager.gameData.team[i]); }


    // ### Tools ###

    private void Next(ref int index, int limit)
    {
        if (index == limit) { index = 0; }
        else { index++; }
    }
    private void Prev(ref int index, int limit)
    {
        if (index == 0) { index = limit; }
        else { index--; }
    }
}
