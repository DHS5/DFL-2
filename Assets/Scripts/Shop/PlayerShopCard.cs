using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShopCard : ShopCard
{
    [Header("Player shop card's specifics")]
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


    public override void GenerateCard(CardSO _cardSO, InventoryManager _inventoryManager, ShopManager _shopManager)
    {
        base.GenerateCard(_cardSO, _inventoryManager, _shopManager);

        PlayerController p = cardSO.prefab.GetComponentInChildren<PlayerController>();
        
        speedSlider.value = p.playerAtt.NormalSpeed;
        sideSpeedSlider.value = p.playerAtt.NormalSideSpeed;
        sprintSlider.value = p.playerAtt.AccelerationM;
        staminaSlider.value = p.playerAtt.accelerationTime / p.playerAtt.accelerationRestTime;
        jumpSlider.value = p.playerAtt.JumpHeight;
        
        dirSensitivitySlider.value = p.playerAtt.DirSensitivity;
        dirGravitySlider.value = p.playerAtt.DirGravity;
        accSensitivitySlider.value = p.playerAtt.AccSensitivity;
        accGravitySlider.value = p.playerAtt.AccGravity;
        
        jukeToggle.isOn = p.playerAtt.CanJuke;
        feintToggle.isOn = p.playerAtt.CanFeint;
        spinToggle.isOn = p.playerAtt.CanSpin;
        slideToggle.isOn = p.playerAtt.CanSlide;
        flipToggle.isOn = p.playerAtt.CanFlip;
        
        skillMovesSlider.value = cardSO.skills;
    }
}
