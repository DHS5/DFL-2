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
    private int CameraPos
    {
        get { return player.playerManager.TpCameraPos; }
        set { player.playerManager.TpCameraPos = value; }
    }

    private void Start()
    {
        // Gets the player's script
        player = GetComponentInParent<Player>();

        // Initializes the camera
        tpCamera = GetComponent<Camera>();

        // Gets the Cursor Manager
        cursor = FindObjectOfType<CursorManager>();

        tpCamera.transform.localPosition = cameraPositions[CameraPos];
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (CameraPos == cameraPositions.Length - 1) CameraPos = 0;
            else CameraPos++;
            tpCamera.transform.localPosition = cameraPositions[CameraPos];
        }
    }
}
