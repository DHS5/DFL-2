using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCapacityShopCard : MonoBehaviour
{
    [Header("Content")]
    public PlayerShopCapacityCardInfo info;

    [Header("UI components")]
    [Header("Physical Zone")]
    [SerializeField] private CapacityCardGauge speedGauge;
    [SerializeField] private CapacityCardGauge sideSpeedGauge;
    [SerializeField] private CapacityCardGauge sprintGauge;
    [SerializeField] private CapacityCardGauge sprintStaminaGauge;
    [SerializeField] private CapacityCardGauge slowGauge;
    [SerializeField] private CapacityCardGauge shiftSpeedGauge;
    [SerializeField] private CapacityCardGauge jumpHeightGauge;
    [SerializeField] private CapacityCardGauge jumpStaminaGauge;

    [Header("Skills Zone")]
    [SerializeField] private Toggle jukeToggle;
    [SerializeField] private Toggle feintToggle;
    [SerializeField] private Toggle spinToggle;
    [SerializeField] private Toggle jukeSpinToggle;
    [SerializeField] private Toggle slideToggle;
    [SerializeField] private Toggle flipToggle;
    [SerializeField] private Toggle truckToggle;
    [SerializeField] private Toggle slideTackleToggle;

    [Header("Handling Zone")]
    [SerializeField] private CapacityCardGauge dirSensitivityGauge;
    [SerializeField] private CapacityCardGauge dirGravityGauge;
    [SerializeField] private CapacityCardGauge accSensitivityGauge;
    [SerializeField] private CapacityCardGauge accGravityGauge;


    public void ApplyInfos(PlayerShopCapacityCardInfo infos)
    {
        speedGauge.ApplyGaugeInfo(infos.speedInfo);
        sideSpeedGauge.ApplyGaugeInfo(infos.sideSpeedInfo);
        sprintGauge.ApplyGaugeInfo(infos.sprintInfo);
        sprintStaminaGauge.ApplyGaugeInfo(infos.staminaInfo);
        slowGauge.ApplyGaugeInfo(infos.slowInfo);
        shiftSpeedGauge.ApplyGaugeInfo(infos.shiftInfo);
        jumpHeightGauge.ApplyGaugeInfo(infos.jumpHeightInfo);
        jumpStaminaGauge.ApplyGaugeInfo(infos.jumpStaminaInfo);

        jukeToggle.isOn = infos.canJuke;
        feintToggle.isOn = infos.canFeint;
        spinToggle.isOn = infos.canSpin;
        jukeSpinToggle.isOn = infos.canJukeSpin;
        slideToggle.isOn = infos.canSlide;
        flipToggle.isOn = infos.canFlip;
        truckToggle.isOn = infos.canTruck;
        slideTackleToggle.isOn = infos.canSlideTackle;

        dirSensitivityGauge.ApplyGaugeInfo(infos.dirSensitivityInfo);
        dirGravityGauge.ApplyGaugeInfo(infos.dirGravityInfo);
        accSensitivityGauge.ApplyGaugeInfo(infos.accSensitivityInfo);
        accGravityGauge.ApplyGaugeInfo(infos.accGravityInfo);
    }

    public void ApplyInfos()
    {
        ApplyInfos(info);
    }
}

[System.Serializable]
public class PlayerShopCapacityCardInfo
{
    [Header("Physical")]
    public CapacityCardGaugeInfo speedInfo;
    public CapacityCardGaugeInfo sideSpeedInfo;
    public CapacityCardGaugeInfo sprintInfo;
    public CapacityCardGaugeInfo staminaInfo;
    public CapacityCardGaugeInfo slowInfo;
    public CapacityCardGaugeInfo shiftInfo;
    public CapacityCardGaugeInfo jumpHeightInfo;
    public CapacityCardGaugeInfo jumpStaminaInfo;

    [Header("Skills")]
    public bool canJuke;
    public bool canFeint;
    public bool canSpin;
    public bool canJukeSpin;
    public bool canSlide;
    public bool canFlip;
    public bool canTruck;
    public bool canSlideTackle;

    [Header("Handling")]
    public CapacityCardGaugeInfo dirSensitivityInfo;
    public CapacityCardGaugeInfo dirGravityInfo;
    public CapacityCardGaugeInfo accSensitivityInfo;
    public CapacityCardGaugeInfo accGravityInfo;
}
