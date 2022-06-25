using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPlayer : MonoBehaviour
{
    [Header("Third Person Player components")]
    [Tooltip("Third person camera")]
    public Camera tPCamera;

    [Tooltip("Animator of the third person player")]
    [HideInInspector] public Animator animator;

    [Tooltip("Third person camera controller")]
    [HideInInspector] public ThirdPersonCameraController tpsCamera;

    [Tooltip("Game Object of the player's right hand")]
    public GameObject rightHand;

    [Tooltip("Game Object of the football")]
    public GameObject football;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        tpsCamera = tPCamera.GetComponent<ThirdPersonCameraController>();
    }
}
