using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackerCapacityShopCard : MonoBehaviour
{
    [Header("Content")]
    public AttackerCapacityShopCardInfo info;

    [Header("UI components")]
    [SerializeField] private TextMeshProUGUI typeText;
    [Space]
    [SerializeField] private CapacityCardGauge defSpeedGauge;
    [SerializeField] private CapacityCardGauge repositionSpeedGauge;
    [SerializeField] private CapacityCardGauge rotationSpeedGauge;
    [SerializeField] private CapacityCardGauge defRotationSpeedGauge;
    [Space]
    [SerializeField] private CapacityCardGauge anticipationGauge;
    [SerializeField] private CapacityCardGauge reactivityGauge;
    [SerializeField] private CapacityCardGauge proximityGauge;
    [SerializeField] private CapacityCardGauge defProximityGauge;


    public void ApplyInfos(AttackerCapacityShopCardInfo infos)
    {
        typeText.text = infos.type;

        defSpeedGauge.ApplyGaugeInfo(infos.defSpeedInfo);
        repositionSpeedGauge.ApplyGaugeInfo(infos.repositionSpeedInfo);
        rotationSpeedGauge.ApplyGaugeInfo(infos.rotSpeedInfo);
        defRotationSpeedGauge.ApplyGaugeInfo(infos.defRotSpeedInfo);

        anticipationGauge.ApplyGaugeInfo(infos.anticipationInfo);
        reactivityGauge.ApplyGaugeInfo(infos.reactivityInfo);
        proximityGauge.ApplyGaugeInfo(infos.proximityInfo);
        defProximityGauge.ApplyGaugeInfo(infos.defProximityInfo);
    }

    public void ApplyInfos(AttackerAttributesSO att)
    {
        info.type = att.Type.ToString();

        info.defSpeedInfo.value = att.defenseSpeed;
        info.repositionSpeedInfo.value = att.back2PlayerSpeed;
        info.rotSpeedInfo.value = att.rotationSpeed;
        info.defRotSpeedInfo.value = att.defenseRotSpeed;

        info.anticipationInfo.value = att.anticipation;
        info.reactivityInfo.value = 1 - att.reactivity;
        info.proximityInfo.value = att.positionRadius;
        info.defProximityInfo.value = att.defenseDistMultiplier != 0 ? att.defenseDistMultiplier : 1;

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
    public string type;
    [Space]
    public CapacityCardGaugeInfo defSpeedInfo;
    public CapacityCardGaugeInfo repositionSpeedInfo;
    [Space]
    public CapacityCardGaugeInfo rotSpeedInfo;
    public CapacityCardGaugeInfo defRotSpeedInfo;
    [Space]
    public CapacityCardGaugeInfo anticipationInfo;
    public CapacityCardGaugeInfo reactivityInfo;
    public CapacityCardGaugeInfo proximityInfo;
    public CapacityCardGaugeInfo defProximityInfo;
}
