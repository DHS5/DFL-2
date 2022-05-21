using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    
    // ### Properties ###
    public float YMouseSensitivity
    {
        set { }
    }
    public float YSmoothRotation
    {
        set { }
    }


    // ### Functions ###

    public void StartPlayer()
    {
        // Unfreezes the player
        //player.GetComponent<PlayerController>().freeze = false;
        // Makes the player chasable
        //player.GetComponent<PlayerGameplay>().isChasable = true;
        //player.GetComponentInChildren<FirstPersonCameraController>().LockCursor();
    }

    public void StopPlayer()
    {
        // Freezes the player
        //player.GetComponent<PlayerController>().freeze = true;
    }

    public void DeadPlayer()
    {
        // Player freezes
        //player.GetComponent<PlayerController>().freeze = true;
        // Player animator stops
        //playerRunAnimator.SetTrigger("Dead");
    }
}
