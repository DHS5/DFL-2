using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private DataManager dataManager;
    private InventoryManager inventoryManager;

    private List<GameOption> defenderOptions = new List<GameOption>();
    private List<GameOption> zombieOptions = new List<GameOption>();



    [Header("Game Screen")]
    [SerializeField] private GameObject playerSimpleCContainer;
    [SerializeField] private GameObject playerSimpleCardPrefab;
    private List<PlayerSimpleCard> playerSimpleCards = new List<PlayerSimpleCard>();

    [SerializeField] private GameObject stadiumCContainer;
    [SerializeField] private GameObject stadiumCardPrefab;
    private List<StadiumCard> stadiumCards = new List<StadiumCard>();

    [SerializeField] private GameObject infoButtons;



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
        set { dataManager.playerPrefs.stadiumIndex = value; }
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
    public int ParkourIndex
    {
        get { return dataManager.playerPrefs.parkourIndex; }
    }




    // ### Functions ###

    private void Awake()
    {
        InitAttackerCardList();
    }

    private void Start()
    {
        GetManagers();

        settingsManager.GetManagers();

        ActuData();
    }


    public void GetManagers()
    {
        settingsManager = SettingsManager.InstanceSettingsManager;
        dataManager = DataManager.InstanceDataManager;
        inventoryManager = GetComponent<InventoryManager>();
    }


    private void ActuData()
    {
        InfoButtonsOn = settingsManager.InfoButtonsOn;
    }
    private void InitAttackerCardList()
    {
        for (int i = 0; i < attackerCards.Length; i++)
            attackerCards[i] = new List<AttackerCard>();
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
    }

    public void ActuOptions()
    {
        dataManager.gameData.gameOptions = (dataManager.gameData.gameMode == global::GameMode.ZOMBIE) ? zombieOptions : defenderOptions;
    }




    private void GetCard<T>(List<CardSO> cardSOs, GameObject prefab, ref List<T> cards, GameObject container, int index, ref GameObject g) where T : Card
    {
        int i = 0;
        foreach (CardSO cardSO in cardSOs)
        {
            if (inventoryManager.IsInInventory(cardSO.type.GetObject()))
            {
                T card = Instantiate(prefab, container.transform).GetComponent<T>();
                card.cardSO = cardSO;
                if (i != index) card.gameObject.SetActive(false);
                else g = card.cardSO.prefab;

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
        GetCard(dataManager.cardsContainer.playerCards, playerSimpleCardPrefab, ref playerSimpleCards, playerSimpleCContainer, PlayerIndex, ref dataManager.gameData.player);

        // Player complete cards
        GetCard(dataManager.cardsContainer.playerCards, playerCompCardPrefab, ref playerCompCards, playerCompCContainer, PlayerIndex, ref dataManager.gameData.player);

        // Enemy cards
        GetCard(dataManager.cardsContainer.enemyCards, enemyCardPrefab, ref enemyCards, enemyCContainer, EnemyIndex, ref dataManager.gameData.enemy);

        // Attacker cards
        for (int i = 0; i < attackerCContainers.Length; i++)
            GetCard(dataManager.cardsContainer.teamCards, attackerCardPrefab, ref attackerCards[i], attackerCContainers[i], AttackerIndex[i], ref dataManager.gameData.team[i]);

        // Stadium cards
        GetCard(dataManager.cardsContainer.stadiumCards, stadiumCardPrefab, ref stadiumCards, stadiumCContainer, StadiumIndex, ref dataManager.gameData.stadium);

        // Parkour cards
        GetCard(dataManager.cardsContainer.parkourCards, parkourCardPrefab, ref parkourCards, parkourCContainer, ParkourIndex, ref dataManager.gameData.parkour);
    }

    private int NextCard<T>(List<T> cards, int index, ref GameObject g) where T : Card
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        Next(ref index, cards.Count - 1);

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;
        g = cards[index].cardSO.prefab;

        return index;
    }
    private int PrevCard<T>(List<T> cards, int index, ref GameObject g) where T : Card
    {
        bool infoActive = cards[index].InfoActive;
        cards[index].gameObject.SetActive(false);

        Prev(ref index, cards.Count - 1);

        cards[index].gameObject.SetActive(true);
        cards[index].InfoActive = infoActive;
        g = cards[index].cardSO.prefab;

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

    public void NextCardStadium() { StadiumIndex = NextCard(stadiumCards, StadiumIndex, ref dataManager.gameData.stadium); }
    public void PrevCardStadium() { StadiumIndex = PrevCard(stadiumCards, StadiumIndex, ref dataManager.gameData.stadium); }


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
