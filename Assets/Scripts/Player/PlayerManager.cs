using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [Tooltip("Player controller")]
    private PlayerController controller;
    [Tooltip("Player gameplay")]
    private PlayerGameplay gameplay;
    [Tooltip("Player animator")]
    private PlayerAnimator animator;

    [Tooltip("First person camera controller")]
    private FirstPersonCameraController fpsCamera;
    //[Tooltip("Third person camera controller")]


    [HideInInspector] public GameObject player;
    
    // ### Properties ###
    public float YMouseSensitivity
    {
        set { fpsCamera.YMS = value; }
    }
    public float YSmoothRotation
    {
        set { fpsCamera.YSR = value; }
    }



    private void Awake()
    {
        main = GetComponent<MainManager>();

        controller = player.GetComponent<PlayerController>();
        gameplay = player.GetComponent<PlayerGameplay>();
        animator = player.GetComponent<PlayerAnimator>();

        fpsCamera = player.GetComponentInChildren<FirstPersonCameraController>();
    }


    // ### Functions ###

    public void StartPlayer()
    {
        controller.freeze = false; // Unfreezes the player
        gameplay.isChasable = true; // Makes the player chasable
        fpsCamera.LockCursor(); // Locks the cursor
    }

    public void StopPlayer()
    {
        controller.freeze = true; // Freezes the player
    }

    public void DeadPlayer()
    {
        controller.freeze = true; // Player freezes
        // Player animator stops
        //playerRunAnimator.SetTrigger("Dead");
    }
}
