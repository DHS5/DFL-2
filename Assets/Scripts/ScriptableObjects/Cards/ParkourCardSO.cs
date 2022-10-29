using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ParkourEnum { NULL = 0, FIRST, SECOND, THIRD, FOURTH, FIFTH, SIXTH, SEVENTH, EIGHTH, NINTH, TENTH }

[CreateAssetMenu(fileName = "ParkourCard", menuName = "ScriptableObjects/Card/ParkourCard", order = 1)]
public class ParkourCardSO : InventoryCardSO
{
    [Header("Parkour card specifics")]
    public ParkourEnum parkour;
    public Parkour prefab;
    public override object cardObject { get { return parkour; } }
    [Space]
    [Range(0, 10)] public int difficulty;
    public int reward;
    public int baseReward;


    private void OnValidate()
    {
        if (prefab != null)
        {
            parkour = prefab.ParkourNum;
            difficulty = prefab.Difficulty;
            reward = prefab.Reward;
            baseReward = prefab.BaseReward;
        }
    }


    public override void SetActive()
    {
        DataManager.InstanceDataManager.playerPrefs.parkourIndex = (int) parkour - 1;
        DataManager.InstanceDataManager.gameData.parkour = prefab;
    }
}
