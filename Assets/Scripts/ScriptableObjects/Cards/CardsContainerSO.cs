using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public List<EnemyCardSO> rookieEnemyCards;
    public List<EnemyCardSO> proEnemyCards;
    public List<EnemyCardSO> starEnemyCards;
    public List<EnemyCardSO> veteranEnemyCards;
    public List<EnemyCardSO> legendEnemyCards;

    public List<EnemyCardSO> GetCardsByIndex(int index)
    {
        return index switch
        {
            0 => rookieEnemyCards,
            1 => proEnemyCards,
            2 => starEnemyCards,
            3 => veteranEnemyCards,
            4 => legendEnemyCards,
            _ => rookieEnemyCards,
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

    public AttackerCardSO GetAttacker(int index)
    {
        foreach (FrontAttackerCardSO a in frontAttackers) if ((int) a.attacker == index) return a;
        foreach (LSideAttackerCardSO a in lSideAttackers) if ((int) a.attacker == index) return a;
        foreach (RSideAttackerCardSO a in rSideAttackers) if ((int) a.attacker == index) return a;
        foreach (BackAttackerCardSO a in backAttackers) if ((int) a.attacker == index) return a;
        return null;
    }


    public List<AttackerCardSO> GetCardsByIndex(int index)
    {
        return index switch
        {
            0 => frontAttackers.Cast<AttackerCardSO>().ToList(),
            1 => lSideAttackers.Cast<AttackerCardSO>().ToList(),
            2 => rSideAttackers.Cast<AttackerCardSO>().ToList(),
            3 => backAttackers.Cast<AttackerCardSO>().ToList(),
            _ => frontAttackers.Cast<AttackerCardSO>().ToList(),
        };
    }
}
