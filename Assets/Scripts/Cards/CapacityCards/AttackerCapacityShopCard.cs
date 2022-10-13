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
    [SerializeField] private CapacityCardGauge sizeSpeedGauge;
    [Space]
    [SerializeField] private CapacityCardGauge anticipationGauge;
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

        anticipationGauge.ApplyGaugeInfo(infos.anticipationInfo);
        sizeSpeedGauge.ApplyGaugeInfo(infos.sizeInfo);

        reactivityGauge.ApplyGaugeInfo(infos.reactivityInfo);
        proximityGauge.ApplyGaugeInfo(infos.proximityInfo);
        defProximityGauge.ApplyGaugeInfo(infos.defProximityInfo);
    }

    public void ApplyInfos(AttackerAttributesSO att)
    {
        info.speedInfo.value = 0;
        info.defSpeedInfo.value = att.defenseSpeed;
        info.repositionSpeedInfo.value = att.back2PlayerSpeed;

        info.rotSpeedInfo.value = att.rotationSpeed;
        info.defRotSpeedInfo.value = att.defenseRotSpeed;

        info.anticipationInfo.value = att.anticipation;
        info.sizeInfo.value = att.size.y;

        info.reactivityInfo.value = 1 - att.reactivity;
        info.proximityInfo.value = att.positionRadius;
        info.defProximityInfo.value = att.defenseDistMultiplier;

        ApplyInfos();
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
    public CapacityCardGaugeInfo anticipationInfo;
    public CapacityCardGaugeInfo sizeInfo;
    [Space]
    public CapacityCardGaugeInfo reactivityInfo;
    public CapacityCardGaugeInfo proximityInfo;
    public CapacityCardGaugeInfo defProximityInfo;
}
