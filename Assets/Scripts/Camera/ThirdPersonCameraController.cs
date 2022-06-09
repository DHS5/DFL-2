using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Tooltip("Player script")]
    [HideInInspector] public Player player;

    [Tooltip("Third person camera")]
    private Camera tpCamera;

    [Tooltip("Cursor Manager")]
    [HideInInspector] public CursorManager cursor;


    [Tooltip("")]
    [SerializeField] private Vector3[] cameraPositions;
    [Tooltip("")]
    private int cameraPos = 0;

    private void Start()
    {
        // Gets the player's script
        player = GetComponentInParent<Player>();

        // Initializes the camera
        tpCamera = GetComponent<Camera>();

        // Gets the Cursor Manager
        cursor = FindObjectOfType<CursorManager>();


        tpCamera.transform.localPosition = cameraPositions[cameraPos];
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (cameraPos == cameraPositions.Length - 1) cameraPos = 0;
            else cameraPos++;
            tpCamera.transform.localPosition = cameraPositions[cameraPos];
        }
    }
}
