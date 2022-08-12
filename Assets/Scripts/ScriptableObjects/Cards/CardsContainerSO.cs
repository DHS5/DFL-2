using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsContainer", menuName = "ScriptableObjects/Container/Cards", order = 1)]
public class CardsContainerSO : ScriptableObject
{
    public List<CardSO> playerCards;
    public List<CardSO> stadiumCards;
    public List<CardSO> teamCards;
    public List<EnemyCardSO> enemyCards;
    public List<CardSO> parkourCards;
    public List<CardSO> weaponCards;
}
