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

    [HideInInspector] public AudioSource audioSource;



    [Header("First Person components")]
    [Tooltip("Game Object of the first person player")]
    public GameObject FPPlayer;

    [Tooltip("Animator of the first person player")]
    [HideInInspector] public Animator fpAnimator;

    [Tooltip("First person camera controller")]
    [HideInInspector] public FirstPersonCameraController fpsCamera;



    [Header("Third Person components")]
    [Tooltip("Game Object of the third person player")]
    public GameObject TPPlayer;

    public Camera TPCamera;

    [Tooltip("Animator of the third person player")]
    [HideInInspector] public Animator tpAnimator;




    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        gameplay = GetComponent<PlayerGameplay>();

        fpAnimator = FPPlayer.GetComponentInChildren<Animator>();

        tpAnimator = TPPlayer.GetComponentInChildren<Animator>();

        fpsCamera = FPPlayer.GetComponentInChildren<FirstPersonCameraController>();

        audioSource = GetComponent<AudioSource>();
    }

}
