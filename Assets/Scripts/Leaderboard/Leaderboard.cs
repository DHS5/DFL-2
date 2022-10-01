using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [Space]
    [SerializeField] private GameObject leaderboardRowPrefab;


    [HideInInspector] public LeaderboardItem personnalHigh;


    public void SetActive(bool state)
    {
        container.SetActive(state);
    }


    public void Add(LeaderboardItem item)
    {
        Debug.Log(item + "//" + leaderboardRowPrefab + "//" + container);
        LeaderboardRow newRow = Instantiate(leaderboardRowPrefab, container.transform).GetComponent<LeaderboardRow>();
        newRow.Item = item;
        newRow.transform.SetSiblingIndex(item.rank - 1);
    }
}