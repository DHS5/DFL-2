using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsContainer", menuName = "ScriptableObjects/Container/Cards", order = 1)]
public class CardsContainerSO : ScriptableObject
{
    public List<PlayerCardSO> playerCards;
    public List<StadiumCardSO> stadiumCards;
    public TeamCardsContainer teamCards;
    public EnemyCardsContainer enemyCards;
    public List<ParkourCardSO> parkourCards;
    public List<WeaponCardSO> weaponCards;
}


[System.Serializable]
public class EnemyCardsContainer
{
    public List<EnemyCardSO> easyEnemyCards;
    public List<EnemyCardSO> normalEnemyCards;
    public List<EnemyCardSO> hardEnemyCards;

    public List<EnemyCardSO> GetCardsByIndex(int index)
    {
        return index switch
        {
            0 => easyEnemyCards,
            1 => normalEnemyCards,
            2 => hardEnemyCards,
            _ => easyEnemyCards,
        };
    }
}


[System.Serializable]
public class TeamCardsContainer
{
    [Header("Team cards")]
    public List<FrontAttackerCardSO> frontAttackers;
    public List<LSideAttackerCardSO> lSideAttackers;
    public List<RSideAttackerCardSO> rSideAttackers;
    public List<BackAttackerCardSO> backAttackers;
}
