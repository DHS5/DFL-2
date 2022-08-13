using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsContainer", menuName = "ScriptableObjects/Container/Cards", order = 1)]
public class CardsContainerSO : ScriptableObject
{
    public List<PlayerCardSO> playerCards;
    public List<StadiumCardSO> stadiumCards;
    public List<AttackerCardSO> teamCards;
    public List<EnemyCardSO> enemyCards;
    public List<ParkourCardSO> parkourCards;
    public List<WeaponCardSO> weaponCards;
}
