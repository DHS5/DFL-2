using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private GameObject[] enemyCContainers;
    [SerializeField] private GameObject enemyCardPrefab;
    private List<EnemyCard>[] enemyCards = new List<EnemyCard>[3];


    [Header("Team Choice Screen")]
    [SerializeField] private TMP_Dropdown attackerPositionDropdown;
    [SerializeField] private GameObject[] attackerCContainers;
    [SerializeField] private GameObject attackerCardPrefab;
    private List<AttackerCard>[] attackerCards = new List<AttackerCard>[4];
    private int[] attackerChoiceIndex = { 0, 0, 0, 0 };


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
    public int[] EnemyIndex
    {
        get { return DataManager.playerPrefs.enemyIndex; }
        set { DataManager.playerPrefs.enemyIndex = value; }
    }

    public int AttackerPosition
    {
        get { return attackerPositionDropdown.value; }
        set
        { 
            CloseAttackerContainers();
            attackerCContainers[value].SetActive(true);
        }
    }
    private int AttackerChoiceIndex
    {
        get { return attackerChoiceIndex[AttackerPosition]; }
        set { attackerChoiceIndex[AttackerPosition] = value; }
    }
    public AttackerCard CurrentCard
    {
        get { return attackerCards[AttackerPosition][AttackerChoiceIndex]; }
    }

    public int ParkourIndex
    {
        get { return DataManager.playerPrefs.parkourIndex; }
    }



    // ### Built-in ###

    private void Awake()
    {
        main = GetComponent<MenuMainManager>();

        InitCardLists();
    }


    // ### Functions ###
    private void InitCardLists()
    {
        for (int i = 0; i < enemyCards.Length; i++)
            enemyCards[i] = new List<EnemyCard>();
        for (int i = 0; i < attackerCards.Length; i++)
            attackerCards[i] = new List<AttackerCard>();
    }


    private void GetCard<T,U>(List<U> cardSOs, GameObject prefab, ref List<T> cards, GameObject container, int index) where T : Card where U : CardSO
    {
        DestroyCards(container);
        cards.Clear();

        int i = 0;
        foreach (CardSO cardSO in cardSOs)
        {
            object obj = cardSO as InventoryCardSO != null ? (cardSO as InventoryCardSO).cardObject : null;
            if (main.InventoryManager.IsInInventory(obj))
            {
                T card = Instantiate(prefab, container.transform).GetComponent<T>();
                card.cardSO = cardSO;
                if (i != index) card.gameObject.SetActive(false);
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

    public void GetCards()
    {
        // Player simple cards
        GetCard(DataManager.cardsContainer.playerCards, playerSimpleCardPrefab, ref playerSimpleCards, playerSimpleCContainer, PlayerIndex);

        // Player complete cards
        GetCard(DataManager.cardsContainer.playerCards, playerCompCardPrefab, ref playerCompCards, playerCompCContainer, PlayerIndex);

        // Enemy cards
        for (int i = enemyCContainers.Length - 1; i >= 0; i--)
            GetCard(DataManager.cardsContainer.enemyCards.GetCardsByIndex(i), enemyCardPrefab, ref enemyCards[i], enemyCContainers[i], EnemyIndex[i]);

        // Attacker cards
        for (int i = 0; i < attackerCContainers.Length; i++)
            GetCard(DataManager.cardsContainer.teamCards.GetCardsByIndex(i), attackerCardPrefab, ref attackerCards[i], attackerCContainers[i], 0);
        AttackerPosition = 0;

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
    public void OpenCardContainerEnemy()
    {
        for (int i = 0; i < enemyCContainers.Length; i++)
        {
            enemyCContainers[i].SetActive(i == (int)DataManager.gameData.gameDifficulty);
        }
    }
    public void NextCardEnemy()
    {
        int i = (int)DataManager.gameData.gameDifficulty;
        EnemyIndex[i] = NextCard(enemyCards[i], EnemyIndex[i]);
        DataManager.gameData.enemy = enemyCards[i][EnemyIndex[i]].enemyCardSO.attribute;
    }
    public void PrevCardEnemy()
    {
        int i = (int)DataManager.gameData.gameDifficulty;
        EnemyIndex[i] = PrevCard(enemyCards[i], EnemyIndex[i]);
        DataManager.gameData.enemy = enemyCards[i][EnemyIndex[i]].enemyCardSO.attribute;
    }

    public void NextCardAttacker()
    {
        AttackerChoiceIndex = NextCard(attackerCards[AttackerPosition], AttackerChoiceIndex);
    }
    public void PrevCardAttacker()
    {
        AttackerChoiceIndex = PrevCard(attackerCards[AttackerPosition], AttackerChoiceIndex);
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

    private void DestroyCards(GameObject container)
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).GetComponent<Card>() != null)
                Destroy(container.transform.GetChild(i).gameObject);
        }
    }

    private void CloseAttackerContainers()
    {
        foreach (GameObject g in attackerCContainers)
            g.SetActive(false);
    }
}
