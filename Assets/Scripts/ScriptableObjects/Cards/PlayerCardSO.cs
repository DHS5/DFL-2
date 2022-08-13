using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum PlayerEnum { NULL, OBJ, LAMAR, KITTLE }

[CreateAssetMenu(fileName = "PlayerCard", menuName = "ScriptableObjects/Card/PlayerCard", order = 1)]
public class PlayerCardSO : ShopCardSO
{
    [Header("Player card specifics")]
    public PlayerEnum player;

    public GameObject prefab;
    public override object cardObject { get { return player; } }

    [Range(0, 10)] public int physical;
    [Range(0, 10)] public int handling;
    [Range(0, 5)] public int skills;


    public override void SetActive()
    {
        DataManager.InstanceDataManager.gameData.player = prefab;
    }
}
