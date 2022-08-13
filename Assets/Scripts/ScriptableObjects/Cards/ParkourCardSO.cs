using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ParkourEnum { NULL = 0, MEDIUM = 1 }

[CreateAssetMenu(fileName = "ParkourCard", menuName = "ScriptableObjects/Card/ParkourCard", order = 1)]
public class ParkourCardSO : InventoryCardSO
{
    [Header("Parkour card specifics")]
    public ParkourEnum parkour;
    public GameObject prefab;
    public override object cardObject { get { return parkour; } }
    [Space]
    public int difficulty;


    public override void SetActive()
    {
        DataManager.InstanceDataManager.gameData.parkour = prefab;
    }
}
