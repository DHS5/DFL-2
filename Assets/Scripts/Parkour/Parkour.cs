using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parkour : MonoBehaviour
{
    [SerializeField] private ParkourEnum parkour;
    public ParkourEnum ParkourNum { get { return parkour; } }

    [SerializeField] private int reward;
    public int Reward { get { return reward; } }

    [SerializeField] [Range(1, 10)] private int difficulty;
    public int Difficulty { get { return difficulty; } }
}
