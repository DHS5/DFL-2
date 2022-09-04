using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCapacityCard : MonoBehaviour
{
    [Header("Content")]
    public WeaponCapacityCardInfo info;

    [Header("UI components")]
    [SerializeField] private CapacityCardGauge rangeGauge;
    [SerializeField] private CapacityCardGauge angleGauge;
    [SerializeField] private CapacityCardGauge ammunitionGauge;
    [SerializeField] private CapacityCardGauge reloadGauge;
    [SerializeField] private CapacityCardGauge maxVictimGauge;


    public void ApplyInfos(WeaponCapacityCardInfo infos)
    {
        rangeGauge.ApplyGaugeInfo(infos.rangeInfo);
        angleGauge.ApplyGaugeInfo(infos.angleInfo);
        ammunitionGauge.ApplyGaugeInfo(infos.ammunitionInfo);
        reloadGauge.ApplyGaugeInfo(infos.reloadInfo);
        maxVictimGauge.ApplyGaugeInfo(infos.maxVictimInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }
}

[System.Serializable]
public class WeaponCapacityCardInfo
{
    public CapacityCardGaugeInfo rangeInfo;
    public CapacityCardGaugeInfo angleInfo;
    public CapacityCardGaugeInfo ammunitionInfo;
    public CapacityCardGaugeInfo reloadInfo;
    public CapacityCardGaugeInfo maxVictimInfo;
}
