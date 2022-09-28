using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerCompleteCard : Card
{
    [Header("Player complete card's specifics")]
    [Header("Physical")]
    [Tooltip("")]
    [SerializeField] private Slider speedSlider;
    [Tooltip("")]
    [SerializeField] private Slider sideSpeedSlider;
    [Tooltip("")]
    [SerializeField] private Slider sprintSlider;
    [Tooltip("")]
    [SerializeField] private Slider staminaSlider;
    [Tooltip("")]
    [SerializeField] private Slider jumpSlider;

    [Header("Handling")]
    [Tooltip("")]
    [SerializeField] private Slider dirSensitivitySlider;
    [Tooltip("")]
    [SerializeField] private Slider dirGravitySlider;
    [Tooltip("")]
    [SerializeField] private Slider accSensitivitySlider;
    [Tooltip("")]
    [SerializeField] private Slider accGravitySlider;


    [Header("Skills")]
    [Tooltip("")]
    [SerializeField] private Slider skillMovesSlider;
    [Tooltip("")]
    [SerializeField] private Toggle jukeToggle;
    [Tooltip("")]
    [SerializeField] private Toggle feintToggle;
    [Tooltip("")]
    [SerializeField] private Toggle spinToggle;
    [Tooltip("")]
    [SerializeField] private Toggle slideToggle;
    [Tooltip("")]
    [SerializeField] private Toggle flipToggle;
    [Tooltip("")]
    [SerializeField] private Toggle extraLifeToggle;


    public PlayerCardSO playerCardSO { get { return cardSO as PlayerCardSO; } }

    protected override void Start()
    {
        base.Start();

        PlayerAttributesSO att = playerCardSO.playerInfo.attributes;

        speedSlider.value = att.NormalSpeed;
        sideSpeedSlider.value = att.NormalSideSpeed;
        sprintSlider.value = att.AccelerationM;
        staminaSlider.value = att.accelerationTime / att.accelerationRestTime;
        jumpSlider.value = att.JumpHeight;

        dirSensitivitySlider.value = att.DirSensitivity;
        dirGravitySlider.value = att.DirGravity;
        accSensitivitySlider.value = att.AccSensitivity;
        accGravitySlider.value = att.AccGravity;

        jukeToggle.isOn = att.CanJuke;
        feintToggle.isOn = att.CanFeint;
        spinToggle.isOn = att.CanSpin;
        slideToggle.isOn = att.CanSlide;
        flipToggle.isOn = att.CanFlip;

        skillMovesSlider.value = playerCardSO.skills;
    }
}
