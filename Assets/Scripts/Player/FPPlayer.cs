using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPlayer : MonoBehaviour
{
    [Header("First Person Player components")]
    [Tooltip("Animator of the first person player")]
    [HideInInspector] public Animator animator;

    [Tooltip("First person camera controller")]
    [HideInInspector] public FirstPersonCameraController fpsCamera;

    [Tooltip("Game Object of the football")]
    public GameObject fpFootball;



    private void Awake()
    {
        animator = GetComponent<Animator>();

        fpsCamera = GetComponentInChildren<FirstPersonCameraController>();
    }
}
