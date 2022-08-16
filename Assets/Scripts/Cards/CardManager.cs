using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private MenuMainManager main;

    private DataManager DataManager
    {
        get { return main.DataManager; }
    }


    [Header("Game Screen")]
    [SerializeField] private GameObject playerSimpleCContainer;
    [SerializeField] private GameObject playerSimpleCardPrefab;
    private List<PlayerSimpleCard> playerSimpleCards = new List<PlayerSimpleCard>();

    [SerializeField] private GameObject stadiumCContainer;
    [SerializeField] private GameObject stadiumCardPrefab;
    private List<StadiumCard> stadiumCards = new List<StadiumCard>();


    [Header("Player Choice Screen")]
    [SerializeField] private GameObject playerCompCContainer;
    [SerializeField] private GameObject playerCompCardPrefab;
    private List<PlayerCompleteCard> playerCompCards = new List<PlayerCompleteCard>();


    [Header("Enemy Choice Screen")]
    [SerializeField] private GameObject enemyCContainer;
    [SerializeField] private GameObject enemyCardPrefab;
    private List<EnemyCard> enemyCards = new List<EnemyCard>();


    [Header("Team Choice Screen")]
    [SerializeField] private GameObject[] attackerCContainers;
    [SerializeField] private GameObject attackerCardPrefab;
    private List<AttackerCard>[] attackerCards = new List<AttackerCard>[5];


    [Header("Parkour Choice Screen")]
    [SerializeField] private GameObject parkourCContainer;
    [SerializeField] private GameObject parkourCardPrefab;
    private List<ParkourCard> parkourCards = new List<ParkourCard>();



    // ### Properties ###

    public int PlayerIndex
    {
        get { return DataManager.playerPrefs.playerIndex; }
        set { DataManager.playerPrefs.playerIndex = value; }
    }
    public int StadiumIndex
    {
        get { return DataManager.playerPrefs.stadiumIndex; }
        set { DataManager.playerPrefs.stadiumIndex = value; }
    }
    public int EnemyIndex
    {
        get { return DataManager.playerPrefs.enemyIndex; }
        set { DataManager.playerPrefs.enemyIndex = value; }
    }
    public int[] AttackerIndex
    {
        get { return DataManager.playerPrefs.teamIndex; }
        set { DataManager.playerPrefs.teamIndex = value; }
    }
    public int ParkourIndex
    {
        get { return DataManager.playerPrefs.parkourIndex; }
    }



    // ### Built-in ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();

        InitAttackerCardList();
    }


    // ### Functions ###
    private void InitAttackerCardList()
    {
        for (int i = 0; i < attackerCards.Length; i++)
            attackerCards[i] = new List<AttackerCard>();
    }


    private void GetCard<T,U>(List<U> cardSOs, GameObject prefab, ref List<T> cards, GameObject container, int index, int teamIndex) where T : Card where U : CardSO
    {
        int i = 0;
        foreach (CardSO cardSO in cardSOs)
        {
            object obj = cardSO as InventoryCardSO != null ? (cardSO as InventoryCardSO).cardObject : null;
            if (main.InventoryManager.IsInInventory(obj))
            {
                T card = Instantiate(prefab, container.transform).GetComponent<T>();
                card.cardSO = cardSO;
                if (i != index) card.gameObject.SetActive(false);
                else if (teamIndex != -1) card.cardSO.SetActive(teamIndex);
                else card.cardSO.SetActive();

                if (card as ParkourCard != null)
                {
                    (card as ParkourCard).GetIndex(i);
                    if (i == index) (card as ParkourCard).On();
                }
                cards.Add(card);
                i++;
            }
        }
    }
    private void GetCard<T,U>(List<U> cardSOs, GameObject prefab, ref List<T> cards, GameObject container, int index) where T : Card where U : CardSO
    {
        GetCard(cardSOs, prefab, ref cards, container, index, -1);
    }

    public void GetCards()
    {
        // Player simple cards
        GetCard(DataManager.cardsContainer.playerCards, playerSimpleCardPrefab, ref playerSimpleCards, playerSimpleCContainer, PlayerIndex);

        // Player complete cards
        GetCard(DataManager.cardsContainer.playerCards, playerCompCardPrefab, ref playerCompCards, playerCompCContainer, PlayerIndex);

        // Enemy cards
        GetCard(DataManager.cardsContainer.enemyCards, enemyCardPrefab, ref enemyCards, enemyCContainer, EnemyIndex);

        // Attacker cards
        for (int i = 0; i < attackerCContainers.Length; i++)
            GetCard(DataManager.cardsContainer.teamCards, attackerCardPrefab, ref attackerCards[i], attackerCContainers[i], AttackerIndex[i], i);

        // Stadium cards
        GetCard(DataManager.cardsContainer.stadiumCards, stadiumCardPrefab, ref stadiumCards, stadiumCContainer, StadiumIndex);

        // Parkour cards
        GetCard(DataManager.cardsContainer.parkourCards, parkourCardPrefab, ref parkourCards, parkourCContainer, ParkourIndex);
    }

    private int NextCard<T>(List<T> cards, int index) where T : Card
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        Next(ref index, cards.Count - 1);

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;

        return index;
    }
    private int PrevCard<T>(List<T> cards, int index) where T : Card
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        Prev(ref index, cards.Count - 1);

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;

        return index;
    }


    public void NextCardPlayer()
    {
        NextCard(playerCompCards, PlayerIndex);
        PlayerIndex = NextCard(playerSimpleCards, PlayerIndex);
        DataManager.gameData.player = playerSimpleCards[PlayerIndex].playerCardSO.prefab;

    }
    public void PrevCardPlayer()
    {
        PrevCard(playerCompCards, PlayerIndex);
        PlayerIndex = PrevCard(playerSimpleCards, PlayerIndex);
        DataManager.gameData.player = playerSimpleCards[PlayerIndex].playerCardSO.prefab;
    }
    public void NextCardEnemy()
    {
        EnemyIndex = NextCard(enemyCards, EnemyIndex);
        DataManager.gameData.enemy = enemyCards[EnemyIndex].enemyCardSO.attribute;
    }
    public void PrevCardEnemy()
    {
        EnemyIndex = PrevCard(enemyCards, EnemyIndex);
        DataManager.gameData.enemy = enemyCards[EnemyIndex].enemyCardSO.attribute;
    }

    public void NextCardAttacker(int i) 
    {
        AttackerIndex[i] = NextCard(attackerCards[i], AttackerIndex[i]);
        DataManager.gameData.team[i] = attackerCards[i][AttackerIndex[i]].attackerCardSO.prefab;
    }
    public void PrevCardAttacker(int i) 
    { 
        AttackerIndex[i] = PrevCard(attackerCards[i], AttackerIndex[i]);
        DataManager.gameData.team[i] = attackerCards[i][AttackerIndex[i]].attackerCardSO.prefab;
    }

    public void NextCardStadium() 
    { 
        StadiumIndex = NextCard(stadiumCards, StadiumIndex);
        DataManager.gameData.stadium = stadiumCards[StadiumIndex].stadiumCardSO.prefab;
    }
    public void PrevCardStadium() 
    { 
        StadiumIndex = PrevCard(stadiumCards, StadiumIndex);
        DataManager.gameData.stadium = stadiumCards[StadiumIndex].stadiumCardSO.prefab;
    }



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
