using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    [HideInInspector] public PlayerManager playerManager;

    [Tooltip("Player controller")]
    [HideInInspector] public PlayerController controller;
    [Tooltip("Player gameplay")]
    [HideInInspector] public PlayerGameplay gameplay;
    [Tooltip("Player effects")]
    [HideInInspector] public PlayerEffects effects;

    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public GameObject activeBody;



    [Header("First Person components")]
    [Tooltip("Game Object of the first person player")]
    public GameObject FPPlayer;

    [Tooltip("Animator of the first person player")]
    [HideInInspector] public Animator fpAnimator;

    [Tooltip("First person camera controller")]
    [HideInInspector] public FirstPersonCameraController fpsCamera;

    [Tooltip("Game Object of the football")]
    public GameObject fpFootball;



    [Header("Third Person components")]
    [Tooltip("Game Object of the third person player")]
    public GameObject TPPlayer;

    public Camera TPCamera;

    [Tooltip("Animator of the third person player")]
    [HideInInspector] public Animator tpAnimator;

    [Tooltip("Third person camera controller")]
    [HideInInspector] public ThirdPersonCameraController tpsCamera;

    [Tooltip("Game Object of the football")]
    public GameObject tpFootball;



    [Header("Player level attributes")]
    [Tooltip("")]
    [Range(1, 10)] public int physical;
    [Tooltip("")]
    [Range(1, 10)] public int handling;
    [Tooltip("")]
    [Range(1, 5)] public int skills;



    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        gameplay = GetComponent<PlayerGameplay>();

        effects = GetComponent<PlayerEffects>();

        fpAnimator = FPPlayer.GetComponentInChildren<Animator>();

        tpAnimator = TPPlayer.GetComponentInChildren<Animator>();

        fpsCamera = FPPlayer.GetComponentInChildren<FirstPersonCameraController>();

        tpsCamera = TPPlayer.GetComponentInChildren<ThirdPersonCameraController>();

        audioSource = GetComponent<AudioSource>();
    }

}
