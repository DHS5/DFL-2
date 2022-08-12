using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCard", menuName = "ScriptableObjects/EnemyCard", order = 1)]
public class EnemyCardSO : CardSO
{
    public DefenderAttributesSO attribute;

    private void OnValidate()
    {
        if (attribute != null)
            Title = attribute.enemyName;
    }
}
