using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public FieldManager fieldManager;

    [HideInInspector] public PlayerManager playerManager;

    [Tooltip("Player controller")]
    [HideInInspector] public PlayerController controller;
    [Tooltip("Player gameplay")]
    [HideInInspector] public PlayerGameplay gameplay;
    [Tooltip("Player effects")]
    [HideInInspector] public PlayerEffects effects;

    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public GameObject activeBody;



    [Header("First person player")]
    [Tooltip("First person player")]
    [HideInInspector] public FPPlayer fPPlayer;



    [Header("Third Person player")]
    [Tooltip("Third person player")]
    [HideInInspector] public TPPlayer tPPlayer;



    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        gameplay = GetComponent<PlayerGameplay>();

        effects = GetComponent<PlayerEffects>();

        fPPlayer = GetComponentInChildren<FPPlayer>();

        tPPlayer = GetComponentInChildren<TPPlayer>();

        audioSource = GetComponent<AudioSource>();
    }

}
