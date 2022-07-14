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


    protected override void Start()
    {
        base.Start();

        image.sprite = cardSO.sprite100x120;

        PlayerController p = cardSO.prefab.GetComponent<PlayerController>();

        speedSlider.value = p.NormalSpeed;
        sideSpeedSlider.value = p.NormalSideSpeed;
        sprintSlider.value = p.AccelerationM;
        staminaSlider.value = p.accelerationTime / p.accelerationRestTime;
        jumpSlider.value = p.JumpHeight;

        dirSensitivitySlider.value = p.DirSensitivity;
        dirGravitySlider.value = p.DirGravity;
        accSensitivitySlider.value = p.AccSensitivity;
        accGravitySlider.value = p.AccGravity;

        jukeToggle.isOn = p.CanJuke;
        feintToggle.isOn = p.CanFeint;
        spinToggle.isOn = p.CanSpin;
        slideToggle.isOn = p.CanSlide;
        flipToggle.isOn = p.CanFlip;

        skillMovesSlider.value = cardSO.skills;
    }
}
