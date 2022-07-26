using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private RectTransform dropdownContainer;
    [SerializeField] private GameObject container;
    [Space]
    [SerializeField] private GameObject leaderboardRowPrefab;


    [HideInInspector] public LeaderboardItem personnalHigh;


    private float rowHeight;


    public void Add(LeaderboardItem item, bool safe)
    {
        if (!safe || !AlreadyIn(item))
        {
            GetRowHeight();

            LeaderboardRow newRow = Instantiate(leaderboardRowPrefab, container.transform).GetComponent<LeaderboardRow>();
            newRow.Item = item;
            newRow.transform.SetSiblingIndex(item.rank - 1);

            dropdownContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownContainer.rect.height + rowHeight);

            // If not already in and safe --> Search for the same player and destroys
            if (safe)
            {
                for (int i = item.rank; i < container.transform.childCount; i++)
                {
                    if (container.transform.GetChild(i).GetComponent<LeaderboardRow>().Item.name == item.name)
                        Destroy(container.transform.GetChild(i));
                }
            }
        }
    }


    private bool AlreadyIn(LeaderboardItem item)
    {
        return CompareItems(container.transform.GetChild(item.rank).GetComponent<LeaderboardRow>().Item, item);
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


    private void GetRowHeight()
    {
        rowHeight = leaderboardRowPrefab.GetComponent<RectTransform>().rect.height;
    }
}