using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerCapacityShopCard : MonoBehaviour
{
    [Header("Content")]
    public AttackerCapacityShopCardInfo info;

    [Header("UI components")]
    [SerializeField] private CapacityCardGauge speedGauge;
    [SerializeField] private CapacityCardGauge defSpeedGauge;
    [SerializeField] private CapacityCardGauge repositionSpeedGauge;
    [Space]
    [SerializeField] private CapacityCardGauge rotationSpeedGauge;
    [SerializeField] private CapacityCardGauge defRotationSpeedGauge;
    [Space]
    [SerializeField] private CapacityCardGauge accSpeedGauge;
    [SerializeField] private CapacityCardGauge sizeSpeedGauge;
    [Space]
    [SerializeField] private CapacityCardGauge reactivityGauge;
    [SerializeField] private CapacityCardGauge proximityGauge;
    [SerializeField] private CapacityCardGauge defProximityGauge;


    public void ApplyInfos(AttackerCapacityShopCardInfo infos)
    {
        speedGauge.ApplyGaugeInfo(infos.speedInfo);
        defSpeedGauge.ApplyGaugeInfo(infos.defSpeedInfo);
        repositionSpeedGauge.ApplyGaugeInfo(infos.repositionSpeedInfo);

        rotationSpeedGauge.ApplyGaugeInfo(infos.rotSpeedInfo);
        defRotationSpeedGauge.ApplyGaugeInfo(infos.defRotSpeedInfo);

        accSpeedGauge.ApplyGaugeInfo(infos.accInfo);
        sizeSpeedGauge.ApplyGaugeInfo(infos.sizeInfo);

        reactivityGauge.ApplyGaugeInfo(infos.reactivityInfo);
        proximityGauge.ApplyGaugeInfo(infos.proximityInfo);
        defProximityGauge.ApplyGaugeInfo(infos.defProximityInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }

}


[System.Serializable]
public class AttackerCapacityShopCardInfo
{
    public CapacityCardGaugeInfo speedInfo;
    public CapacityCardGaugeInfo defSpeedInfo;
    public CapacityCardGaugeInfo repositionSpeedInfo;
    [Space]
    public CapacityCardGaugeInfo rotSpeedInfo;
    public CapacityCardGaugeInfo defRotSpeedInfo;
    [Space]
    public CapacityCardGaugeInfo accInfo;
    public CapacityCardGaugeInfo sizeInfo;
    [Space]
    public CapacityCardGaugeInfo reactivityInfo;
    public CapacityCardGaugeInfo proximityInfo;
    public CapacityCardGaugeInfo defProximityInfo;
}
