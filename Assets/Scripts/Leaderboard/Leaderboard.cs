using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private RectTransform scrollviewContainer;
    [SerializeField] private GameObject container;
    [Space]
    [SerializeField] private GameObject leaderboardRowPrefab;


    [HideInInspector] public LeaderboardItem personnalHigh;


    public float TotalHeight
    {
        get { return container.transform.childCount * RowHeight; }
    }
    private float RowHeight
    {
        get { return leaderboardRowPrefab.GetComponent<RectTransform>().rect.height; }
    }

    public void SetActive(bool state)
    {
        container.SetActive(state);
        if (state) scrollviewContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalHeight);
    }


    public void Add(LeaderboardItem item, bool safe)
    {
        if (true)
        {
            LeaderboardRow newRow = Instantiate(leaderboardRowPrefab, container.transform).GetComponent<LeaderboardRow>();
            newRow.Item = item;
            newRow.transform.SetSiblingIndex(item.rank - 1);

            // If not already in and safe --> Search for the same player and destroys
            //if (safe)
            //{
            //    for (int i = item.rank; i < container.transform.childCount; i++)
            //    {
            //        if (container.transform.GetChild(i).GetComponent<LeaderboardRow>().Item.name == item.name)
            //            Destroy(container.transform.GetChild(i).gameObject);
            //    }
            //}

            scrollviewContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalHeight);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
    }


    private bool AlreadyIn(LeaderboardItem item)
    {
        if (container.transform.childCount <= item.rank - 1) return false;
        return CompareItems(container.transform.GetChild(item.rank - 1).GetComponent<LeaderboardRow>().Item, item);
    }

    private bool CompareItems(LeaderboardItem item1, LeaderboardItem item2)
    {
        if (item1.name != item2.name) return false;
        if (item1.score != item2.score) return false;
        if (item1.wave != item2.wave) return false;
        if (item1.wheather != item2.wheather) return false;
        if (item1.options != item2.options) return false;

        return true;
    }
}