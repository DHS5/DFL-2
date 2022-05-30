using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    [HideInInspector] public GameManager gameManager;


    [Tooltip("Player controller")]
    [HideInInspector] public PlayerController controller;
    [Tooltip("Player gameplay")]
    [HideInInspector] public PlayerGameplay gameplay;
    [Tooltip("Player animator")]
    [HideInInspector] public Animator playerAnimator;

    [Tooltip("First person camera controller")]
    [HideInInspector] public FirstPersonCameraController fpsCamera;
    //[Tooltip("Third person camera controller")]
    [Tooltip("Camera animator")]
    [HideInInspector] public CameraAnimator cameraAnimator;

    [HideInInspector] public AudioSource audioSource;

    [Tooltip("List of player prefabs")]
    [SerializeField] private GameObject[] playerPrefabs;

    [HideInInspector] public GameObject player;
    
    // ### Properties ###
    public float YMouseSensitivity
    {
        set { } // fpsCamera.YMS = value; }
    }
    public float YSmoothRotation
    {
        set { } // fpsCamera.YSR = value; }
    }



    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    public void PreparePlayer()
    {
        gameManager = main.GameManager;

        player = Instantiate(playerPrefabs[main.GameManager.gameData.playerIndex], new Vector3(0, 10, 0), Quaternion.identity);

        controller = player.GetComponent<PlayerController>();
        controller.playerManager = this;

        gameplay = player.GetComponent<PlayerGameplay>();
        gameplay.playerManager = this;

        playerAnimator = player.GetComponentInChildren<Animator>();

        fpsCamera = player.GetComponentInChildren<FirstPersonCameraController>();
        cameraAnimator = player.GetComponentInChildren<CameraAnimator>();

        audioSource = player.GetComponent<AudioSource>();
    }

    public void StartPlayer()
    {
        gameplay.freeze = false; // Unfreezes the player
        gameplay.isChasable = true; // Makes the player chasable
        //fpsCamera.LockCursor(); // Locks the cursor
    }

    public void StopPlayer()
    {
        gameplay.freeze = true; // Freezes the player
    }

    public void DeadPlayer()
    {
        gameplay.freeze = true; // Player freezes
        // Player animator stops
        //playerRunAnimator.SetTrigger("Dead");
    }
}
