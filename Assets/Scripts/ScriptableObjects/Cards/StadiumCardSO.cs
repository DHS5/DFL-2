using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum StadiumEnum { NULL, TRAINING, POOL, ARROWHEAD, DOME, TIGER }

[CreateAssetMenu(fileName = "StadiumCard", menuName = "ScriptableObjects/Card/StadiumCard", order = 1)]
public class StadiumCardSO : ShopCardSO
{
    [Header("Stadium card specifics")]
    public StadiumEnum stadium;

    public GameObject prefab;
    public override object cardObject { get { return stadium; } }


    public override void SetActive()
    {
        DataManager.InstanceDataManager.gameData.stadium = prefab;
    }
}
