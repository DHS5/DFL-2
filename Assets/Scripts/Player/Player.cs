using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public FieldManager fieldManager;

    [HideInInspector] public PlayerManager playerManager;

    [Tooltip("Player controller")]
    public PlayerController controller;
    [Tooltip("Player gameplay")]
    public PlayerGameplay gameplay;
    [Tooltip("Player effects")]
    public PlayerEffects effects;

    [HideInInspector] public GameObject activeBody;



    [Header("First person player")]
    [Tooltip("First person player")]
    public FPPlayer fPPlayer;



    [Header("Third Person player")]
    [Tooltip("Third person player")]
    public TPPlayer tPPlayer;


    [Header("Audio Sources")]
    public AudioSource[] crowdAudioSources;

    [Header("Zombie light")]
    public GameObject zombieLight;

    public void CreatePlayer(PlayerInfo player, bool zombie)
    {
        fPPlayer.CreateFPPlayer(player.avatar, player.mesh, player.materials);
        tPPlayer.CreateTPPlayer(player.avatar, player.mesh, player.materials);

        controller.playerAtt = player.attributes;

        zombieLight.SetActive(zombie);
    }
}
