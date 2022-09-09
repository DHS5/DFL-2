using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCapacityCard : MonoBehaviour
{
    [Header("Content")]
    public EnemyCapacityCardInfo info;

    [Header("UI components")]
    [Header("Physical Zone")]
    [SerializeField] private CapacityCardGauge speedGauge;
    [SerializeField] private CapacityCardGauge accGauge;
    [SerializeField] private CapacityCardGauge rotSpeedGauge;

    [Header("Behaviour Zone")]
    [SerializeField] private CapacityCardGauge reactivityGauge;
    [SerializeField] private CapacityCardGauge intelligenceGauge;
    [SerializeField] private CapacityCardGauge attackDistGauge;


    public void ApplyInfos(EnemyCapacityCardInfo infos)
    {
        speedGauge.ApplyGaugeInfo(infos.speedInfo);
        accGauge.ApplyGaugeInfo(infos.accInfo);
        rotSpeedGauge.ApplyGaugeInfo(infos.rotSpeedInfo);
        reactivityGauge.ApplyGaugeInfo(infos.reactivityInfo);
        intelligenceGauge.ApplyGaugeInfo(infos.intelligenceInfo);
        attackDistGauge.ApplyGaugeInfo(infos.attackDistInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }
}



[System.Serializable]
public class EnemyCapacityCardInfo
{
    [Header("Physical")]
    public CapacityCardGaugeInfo speedInfo;
    public CapacityCardGaugeInfo accInfo;
    public CapacityCardGaugeInfo rotSpeedInfo;

    [Header("Behaviour")]
    public CapacityCardGaugeInfo reactivityInfo;
    public CapacityCardGaugeInfo intelligenceInfo;
    public CapacityCardGaugeInfo attackDistInfo;
}
