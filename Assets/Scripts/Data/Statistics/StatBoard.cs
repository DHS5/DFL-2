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

    public bool SetActive { set { gameObject.SetActive(value); } }

    public StatsData Data
    {
        get { return data; }
        set
        {
            data = value;
            ApplyData();
        }
    }

    private float TotalHeight
    {
        get { return gaugeContainer.childCount * GaugeHeight; }
    }
    private float GaugeHeight
    {
        get { return waveGaugePrefab.GetComponent<RectTransform>().rect.height; }
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
                gaugeContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalHeight);
            }
            if (data.gameNumber != 0) waveGauges[i].Set((i + 1).ToString(), (float)data.wavesReached[i] / data.gameNumber);
            else waveGauges[i].Set((i + 1).ToString(), 0);
        }
    }
}
