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

    [Tooltip("Game Object of the player's right hand")]
    public GameObject rightHand;

    [Tooltip("Game Object of the football")]
    public GameObject football;

    [Tooltip("First person renderer")]
    public SkinnedMeshRenderer fpRenderer;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        fpsCamera = GetComponentInChildren<FirstPersonCameraController>();
    }

    public void CreateFPPlayer(Avatar avatar, Mesh mesh, Material[] materials)
    {
        animator.avatar = avatar;

        fpRenderer.sharedMesh = mesh;
        fpRenderer.materials = materials;
    }
}
