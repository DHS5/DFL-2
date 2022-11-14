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
        LeaderboardRow newRow = Instantiate(leaderboardRowPrefab, container.transform).GetComponent<LeaderboardRow>();
        newRow.Item = item;
        newRow.transform.SetSiblingIndex(item.rank - 1);
    }

    public void Clear()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
        personnalHigh = new LeaderboardItem { name = "", options = "", rank = 0, score = 0, wave = "0", wheather = "" };
    }
}