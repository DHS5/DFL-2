using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ParkourEnum { NULL = 0, FIRST, SECOND, THIRD, FOURTH, FIFTH, SIXTH, SEVENTH, EIGHTH, NINTH, TENTH }

[CreateAssetMenu(fileName = "ParkourCard", menuName = "ScriptableObjects/Card/ParkourCard", order = 1)]
public class ParkourCardSO : InventoryCardSO
{
    [Header("Parkour card specifics")]
    
    public Parkour prefab;
    public override object cardObject { get { return Parkour; } }
    public ParkourEnum Parkour { get { return prefab.ParkourNum; } }
    public int Difficulty { get { return prefab.Difficulty; } }
    public int Reward { get { return prefab.Reward; } }
    public int BaseReward { get { return prefab.BaseReward; } }

    public bool locked;


    public override void SetActive()
    {
        DataManager.InstanceDataManager.playerPrefs.parkourIndex = (int) Parkour - 1;
        DataManager.InstanceDataManager.gameData.parkour = (int) Parkour - 1;
    }
}
