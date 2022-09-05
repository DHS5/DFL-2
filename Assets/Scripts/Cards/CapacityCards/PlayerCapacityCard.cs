using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCapacityCard : MonoBehaviour
{
    [Header("Content")]
    public PlayerCapacityCardInfo info;

    [Header("UI Components")]
    [SerializeField] private CapacityCardGauge physicalGauge;
    [SerializeField] private CapacityCardGauge handlingGauge;
    [SerializeField] private CapacityCardGauge skillsGauge;



    public void ApplyInfos(PlayerCapacityCardInfo infos)
    {
        physicalGauge.ApplyGaugeInfo(infos.physicalInfo);
        handlingGauge.ApplyGaugeInfo(infos.handlingInfo);
        skillsGauge.ApplyGaugeInfo(infos.skillsInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }
}


[System.Serializable]
public class PlayerCapacityCardInfo
{
    public CapacityCardGaugeInfo physicalInfo;
    public CapacityCardGaugeInfo handlingInfo;
    public CapacityCardGaugeInfo skillsInfo;
}
