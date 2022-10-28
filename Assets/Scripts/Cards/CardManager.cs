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
    [SerializeField] private PlayerCard playerCard;
    private List<PlayerCardSO> playerCards = new();
    [SerializeField] private LockerRoom lockerRoom;
    [Space]
    [SerializeField] private GameObject stadiumCContainer;
    [SerializeField] private GameObject stadiumCardPrefab;
    private List<StadiumCard> stadiumCards = new();


    [Header("Enemy Choice Screen")]
    [SerializeField] private EnemyCard enemyCard;
    private List<EnemyCardSO>[] enemyCards = new List<EnemyCardSO>[5];
    [SerializeField] EnemyLockerRoom enemyLockerRoom;


    [Header("Team Choice Screen")]
    [SerializeField] private TMP_Dropdown attackerPositionDropdown;
    [SerializeField] private AttackerLargeCard attackerLargeCard;
    private List<AttackerCardSO>[] attackerCards = new List<AttackerCardSO>[4];
    private int[] attackerChoiceIndex = { 0, 0, 0, 0 };
    public TeamLockerRoom teamLockerRoom;
    public AttackerLockerRoom attackerLockerRoom;


    [Header("Parkour Choice Screen")]
    [SerializeField] private GameObject parkourCContainer;
    [SerializeField] private GameObject parkourCardPrefab;
    private List<ParkourCard> parkourCards = new();



    // ### Properties ###

    public int PlayerIndex
    {
        get { return DataManager.playerPrefs.playerIndex; }
        set { DataManager.playerPrefs.playerIndex = value; }
    }
    public PlayerCardSO CurrentPlayerCard
    {
        get { return playerCards[PlayerIndex]; }
    }

    public int StadiumIndex
    {
        get { return DataManager.playerPrefs.stadiumIndex; }
        set { DataManager.playerPrefs.stadiumIndex = value; }
    }
    public StadiumCardSO CurrentStadiumCard
    {
        get { return stadiumCards[StadiumIndex].stadiumCardSO; }
    }

    public int[] EnemyIndex
    {
        get { return DataManager.playerPrefs.enemyIndex; }
        set { DataManager.playerPrefs.enemyIndex = value; }
    }
    public EnemyCardSO CurrentEnemyCard
    {
        get 
        {
            int diff = (int)DataManager.gameData.gameDifficulty;
            return enemyCards[diff][EnemyIndex[diff]];
        }
    }

    public int AttackerPosition
    {
        get { return attackerPositionDropdown.value; }
        set { ActuAttacker(); }
    }
    private int AttackerChoiceIndex
    {
        get { return attackerChoiceIndex[AttackerPosition]; }
        set { attackerChoiceIndex[AttackerPosition] = value; }
    }
    public AttackerCardSO CurrentAttackerCard
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
            enemyCards[i] = new List<EnemyCardSO>();
        for (int i = 0; i < attackerCards.Length; i++)
            attackerCards[i] = new List<AttackerCardSO>();
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
    private void GetCard<T>(List<T> cardSOs, ref List<T> cards, int index) where T : CardSO
    {
        cards.Clear();

        int i = 0;
        foreach (T cardSO in cardSOs)
        {
            object obj = cardSO as InventoryCardSO != null ? (cardSO as InventoryCardSO).cardObject : null;
            if (main.InventoryManager.IsInInventory(obj))
            {
                cards.Add(cardSO);
                i++;
            }
        }
    }
    private void GetParkourCard(List<ParkourCardSO> cardSOs, GameObject prefab, ref List<ParkourCard> cards, GameObject container, int index)
    {
        DestroyCards(container);
        cards.Clear();

        int i = 0;
        foreach (ParkourCardSO cardSO in cardSOs)
        {
            object obj = cardSO.cardObject;
            if (main.InventoryManager.IsInInventory(obj))
            {
                ParkourCard card = Instantiate(prefab, container.transform).GetComponent<ParkourCard>();
                card.cardSO = cardSO;
                card.GetIndex(i);
                if (i == index)
                {
                    card.cardSO.SetActive();
                    card.On();
                }
                cards.Add(card);
                i++;
            }
        }
    }

    public void GetCards()
    {
        // Player simple cards
        GetCard(DataManager.cardsContainer.playerCards, ref playerCards, PlayerIndex);
        ActuPlayer();

        // Enemy cards
        for (int i = enemyCards.Length - 1; i >= 0; i--)
            GetCard(DataManager.cardsContainer.enemyCards.GetCardsByIndex(i), ref enemyCards[i], EnemyIndex[i]);
        ActuEnemy();

        // Attacker cards
        for (int i = 0; i < attackerCards.Length; i++)
            GetCard(DataManager.cardsContainer.teamCards.GetCardsByIndex(i), ref attackerCards[i], 0);
        ActuAttacker();

        // Stadium cards
        GetCard(DataManager.cardsContainer.stadiumCards, stadiumCardPrefab, ref stadiumCards, stadiumCContainer, StadiumIndex);
        ActuStadium();

        // Parkour cards
        GetParkourCard(DataManager.cardsContainer.parkourCards, parkourCardPrefab, ref parkourCards, parkourCContainer, ParkourIndex);
    }


    private int NextCard<T>(List<T> cards, int index) where T : Card
    {
        cards[index].gameObject.SetActive(false);

        Next(ref index, cards.Count - 1);

        cards[index].gameObject.SetActive(true);

        return index;
    }
    private int PrevCard<T>(List<T> cards, int index) where T : Card
    {
        cards[index].gameObject.SetActive(false);

        Prev(ref index, cards.Count - 1);

        cards[index].gameObject.SetActive(true);

        return index;
    }

    // Player
    private void ActuPlayer()
    {
        DataManager.gameData.player = CurrentPlayerCard.playerInfo;
        playerCard.ApplyPlayerInfos(CurrentPlayerCard);
        teamLockerRoom.ApplyTeamMaterial(CurrentPlayerCard.playerInfo.attributes.teamMaterial);
        attackerLockerRoom.ApplyTeamMaterial(CurrentPlayerCard.playerInfo.attributes.teamMaterial);
    }
    public void NextCardPlayer()
    {
        PlayerIndex = Next(PlayerIndex, playerCards.Count - 1, true);
        ActuPlayer();
    }
    public void PrevCardPlayer()
    {
        PlayerIndex = Prev(PlayerIndex, playerCards.Count - 1, true);
        ActuPlayer();
    }
    // Enemy
    public void ActuEnemy()
    {
        enemyCard.ApplyEnemyInfos(CurrentEnemyCard);
    }
    public void NextCardEnemy()
    {
        int i = (int)DataManager.gameData.gameDifficulty;
        EnemyIndex[i] = Next(EnemyIndex[i], enemyCards[i].Count - 1, false);
        DataManager.gameData.enemy = enemyCards[i][EnemyIndex[i]].attribute;
        ActuEnemy();
    }
    public void PrevCardEnemy()
    {
        int i = (int)DataManager.gameData.gameDifficulty;
        EnemyIndex[i] = Prev(EnemyIndex[i], enemyCards[i].Count - 1, false);
        DataManager.gameData.enemy = enemyCards[i][EnemyIndex[i]].attribute;
        ActuEnemy();
    }
    // Team
    private void ActuAttacker()
    {
        attackerLargeCard.ApplyCardSOInfo(CurrentAttackerCard);
    }
    public void NextCardAttacker()
    {
        AttackerChoiceIndex = Next(AttackerChoiceIndex, attackerCards[AttackerPosition].Count - 1, false);
        ActuAttacker();
    }
    public void PrevCardAttacker()
    {
        AttackerChoiceIndex = Prev(AttackerChoiceIndex, attackerCards[AttackerPosition].Count - 1, false);
        ActuAttacker();
    }
    // Stadium
    private void ActuStadium()
    {
        DataManager.gameData.stadium = CurrentStadiumCard.prefab;
        enemyLockerRoom.ApplyEnemyMaterial(CurrentStadiumCard.enemyMaterial);
    }
    public void NextCardStadium() 
    { 
        StadiumIndex = NextCard(stadiumCards, StadiumIndex);
        ActuStadium();
    }
    public void PrevCardStadium() 
    { 
        StadiumIndex = PrevCard(stadiumCards, StadiumIndex);
        ActuStadium();
    }



    public void ActuCharacterCards()
    {
        ActuPlayer();
        ActuAttacker();
    }


    // ### Tools ###

    private void Next(ref int index, int limit)
    {
        if (index == limit) { index = 0; }
        else { index++; }
    }
    private int Next(int index, int limit, bool cycle)
    {
        return index == limit ? (cycle ? 0 : index) : index + 1;
    }
    private void Prev(ref int index, int limit)
    {
        if (index == 0) { index = limit; }
        else { index--; }
    }
    private int Prev(int index, int limit, bool cycle)
    {
        return index == 0 ? (cycle ? limit : index) : index - 1;
    }

    private void DestroyCards(GameObject container)
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).GetComponent<Card>() != null)
                Destroy(container.transform.GetChild(i).gameObject);
        }
    }
}
