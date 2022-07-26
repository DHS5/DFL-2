using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardRow : MonoBehaviour
{
    private LeaderboardItem item;

    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI wheatherText;
    [SerializeField] private TextMeshProUGUI optionsText;


    // ### Properties ###

    public LeaderboardItem Item
    {
        get { return item; }
        set { item = value; ApplyLeaderboardItem(); }
    }



    private void ApplyLeaderboardItem()
    {
        rankText.text = item.rank.ToString();
        nameText.text = item.name;
        scoreText.text = item.score.ToString();
        waveText.text = item.wave;
        wheatherText.text = item.wheather;
        optionsText.text = item.options;
    }
}
