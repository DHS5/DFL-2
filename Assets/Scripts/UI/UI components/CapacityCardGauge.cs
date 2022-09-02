using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CapacityCardGauge : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private CapacityCardGaugeInfo info;

    [Header("UI components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Slider gauge;

    private void Awake()
    {
        ApplyGaugeInfo(info);
    }

    public void ApplyGaugeInfo(CapacityCardGaugeInfo infos)
    {
        info = infos;

        titleText.text = infos.title;

        gauge.minValue = infos.minValue;
        gauge.maxValue = infos.maxValue;
        gauge.value = infos.value;
    }
}

[System.Serializable]
public class CapacityCardGaugeInfo
{
    public string title;
    [Space]
    public float minValue;
    public float maxValue;
    public float value;
}