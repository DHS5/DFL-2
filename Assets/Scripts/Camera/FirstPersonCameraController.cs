using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the first person camera attached to the player
/// </summary>
public class FirstPersonCameraController : MonoBehaviour
{
    [Tooltip("Player script")]
    [HideInInspector] public Player player;

    [Tooltip("Player body game object")]
    [SerializeField] private GameObject playerBody;

    [Tooltip("Head game object, parent of the first person camera")]
    [SerializeField] private GameObject head;

    [Tooltip("First person camera")]
    private Camera fpCamera;

    [Tooltip("Cursor Manager")]
    [HideInInspector] public CursorManager cursor;



    [Tooltip("Quaternion containing the camera rotation")]
    private Quaternion cameraRotation;

    [Header("First person camera parameters")]
    [Tooltip("Angle at which the player's head is rotated around the X-axis")]
    [Range(-10, 15)]
    [SerializeField] private float headAngle = 10f;
    [Tooltip("Max angle at which the player is able to look behind")]
    [SerializeField] private int angleMax = 150;
    [Tooltip("Mouse sensitivity along the Y axis")]
    [Range(0.1f, 10f)]
    [SerializeField]  private float yMouseSensitivity = 3f;
    public float YMS { set { yMouseSensitivity = value; } }
    [Tooltip("Mouse smoothness of the rotation")]
    [Range(10, 30)]
    [SerializeField] private float ySmoothRotation = 20f;
    public float YSR { set { ySmoothRotation = value; } }


    [Tooltip("")]
    [SerializeField] private Vector3[] cameraPositions;
    [Tooltip("")]
    private int cameraPos = 0;




    void Start()
    {
        // Gets the player's script
        player = GetComponentInParent<Player>();
        
        // Initializes the camera
        fpCamera = GetComponent<Camera>();

        // Gets the Cursor Manager
        cursor = FindObjectOfType<CursorManager>();

        // Initializes the camera's rotation
        cameraRotation = head.transform.rotation;

        // Gets the camera parameters
        yMouseSensitivity = player.playerManager.YMouseSensitivity;
        ySmoothRotation = player.playerManager.YSmoothRotation;

        fpCamera.transform.localPosition = cameraPositions[cameraPos];
    }


    private void LateUpdate()
    {
        // Gets the look rotation of the camera
        if (cursor.locked && player.gameManager.GameOn) LookRotation();

        if (cursor.locked && Input.GetKeyDown(KeyCode.L))
        {
            if (cameraPos == cameraPositions.Length - 1) cameraPos = 0;
            else cameraPos++;
            fpCamera.transform.localPosition = cameraPositions[cameraPos];
        }
    }


    // ### Functions ###

    /// <summary>
    /// Clamps the rotation around the Y axis in the range [-angleMax,angleMax]
    /// Also keeps the rotation stable during the run
    /// </summary>
    /// <param name="rot">Original quaternion</param>
    /// <returns>Clamped quaternion</returns>
    private Quaternion ClampRotation(Quaternion rot)
    {
        // Normalize the original quaternion
        rot.y /= rot.w;
        rot.w = 1;

        // Keeps it stable around the X and Z axis
        //rot = Quaternion.Euler(headAngle, rot.eulerAngles.y, 0f);

        // Clamps the Y rotation in the angleMax range
        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rot.y);
        angleY = Mathf.Clamp(angleY, -angleMax, angleMax);
        rot.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        // Returns the usable quaternion
        return rot;
    }

    /// <summary>
    /// Makes the camera look in the direction of the cursor
    /// </summary>
    private void LookRotation()
    {
        float xClamp = 1f;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            xClamp = 2f;
        }

        // Gets the mouse X position and clamps it
        float yRotation = Mathf.Clamp(Input.GetAxis("Mouse X"), -xClamp, xClamp) * yMouseSensitivity * 1f;

        // Gets the new camera's rotation
        cameraRotation *= Quaternion.Euler(0f, yRotation, 0f);
        cameraRotation = ClampRotation(cameraRotation);

        // Slerps to the new rotation
        head.transform.localRotation = Quaternion.Slerp(head.transform.localRotation, cameraRotation, ySmoothRotation * Time.deltaTime);
        // Fixes the x-rotation to the head angle and the z-rotation to the body's rotation
        head.transform.rotation = Quaternion.Euler(headAngle, head.transform.rotation.eulerAngles.y, playerBody.transform.rotation.eulerAngles.z);
    }
}
