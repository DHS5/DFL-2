using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackerCapacityCard : MonoBehaviour
{
    [Header("Content")]
    public AttackerCapacityCardInfo info;

    [Header("UI components")]
    [SerializeField] private TextMeshProUGUI positionText;
    [Space]
    [SerializeField] private CapacityCardGauge speedGauge;
    [SerializeField] private CapacityCardGauge rotationSpeedGauge;
    [Space]
    [SerializeField] private CapacityCardGauge proximityGauge;
    [SerializeField] private CapacityCardGauge reactivityGauge;


    private void Awake()
    {
        ApplyInfos();
    }

    public void ApplyInfos(AttackerCapacityCardInfo infos)
    {
        positionText.text = infos.position;

        speedGauge.ApplyGaugeInfo(infos.speedInfo);
        rotationSpeedGauge.ApplyGaugeInfo(infos.rotSpeedInfo);
        proximityGauge.ApplyGaugeInfo(infos.proximityInfo);
        reactivityGauge.ApplyGaugeInfo(infos.reactivityInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }
}

[System.Serializable]
public class AttackerCapacityCardInfo
{
    public string position;
    [Space]
    public CapacityCardGaugeInfo speedInfo;
    public CapacityCardGaugeInfo rotSpeedInfo;
    [Space]
    public CapacityCardGaugeInfo proximityInfo;
    public CapacityCardGaugeInfo reactivityInfo;
}
