using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatBoard : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private TextMeshProUGUI totalGamesText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI averageScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [Space]
    [SerializeField] private RectTransform gaugeContainer;

    [Header("Prefabs")]
    [SerializeField] private GameObject waveGaugePrefab;


    private List<WaveGauge> waveGauges = new();


    private StatsData data;



    // ### Properties ###

    public bool SetActive { set { gameObject.SetActive(value); LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform); } }

    public StatsData Data
    {
        get { return data; }
        set
        {
            data = value;
            ApplyData();
        }
    }

    // ### Functions ###

    /// <summary>
    /// Apply the statistics data to the board UI components
    /// </summary>
    private void ApplyData()
    {
        totalGamesText.text = "Total games played : " + data.gameNumber;
        totalScoreText.text = "Total score : " + data.totalScore;
        bestScoreText.text = "Best score : " + data.bestScore;
        if (data.gameNumber != 0) averageScoreText.text = "Average score : " + (data.totalScore / data.gameNumber);
        else averageScoreText.text = "Average score : 0";

        GaugesCreation();
    }


    private void GaugesCreation()
    {
        for (int i = 0; i < data.wavesReached.Length; i++)
        {
            if (waveGauges.Count <= i)
            {
                waveGauges.Add(Instantiate(waveGaugePrefab, gaugeContainer.transform).GetComponent<WaveGauge>());
                waveGauges[i].transform.SetSiblingIndex(i);

                LayoutRebuilder.ForceRebuildLayoutImmediate(gaugeContainer);
            }
            if (data.gameNumber != 0) waveGauges[i].Set((i + 1).ToString(), data.wavesReached[i].ToString(), (float)data.wavesReached[i] / data.gameNumber);
            else waveGauges[i].Set((i + 1).ToString(), "0", 0);
        }
    }
}
