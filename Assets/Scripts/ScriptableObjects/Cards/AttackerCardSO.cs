using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum AttackerEnum { NULL, FRONT1, BACK1, RSIDE1, LSIDE1, FRONT2 }

[CreateAssetMenu(fileName = "AttackerCard", menuName = "ScriptableObjects/Card/AttackerCard", order = 1)]
public class AttackerCardSO : ShopCardSO
{
    [Header("Attacker card specifics")]
    public AttackerEnum attacker;

    public GameObject prefab;
    public override object cardObject { get { return attacker; } }

    public string position;


    public override void SetActive(int index)
    {
        DataManager.InstanceDataManager.gameData.team[index] = prefab;
    }
}
