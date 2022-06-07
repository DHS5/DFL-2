using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    [HideInInspector] public bool locked;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }

    void Start()
    {
        // Locks the cursor
        LockCursor();
    }

    void Update()
    {
        if (!locked && Input.GetMouseButtonDown(1))
        {
            LockCursor();
        }

        if (locked && (Input.GetKeyDown(KeyCode.Escape) || main.GameManager.GameOver || !main.GameManager.GameOn))
        {
            UnlockCursor();
        }
    }


    // ### Functions ###

    /// <summary>
    /// Locks the cursor and makes it invisible
    /// </summary>
    public void LockCursor()
    {
        // Lock the cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Makes the cursor invisible
        Cursor.visible = false;

        locked = true;
    }

    private void UnlockCursor()
    {
        // Unlock the cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.None;

        // Makes the cursor visible
        Cursor.visible = true;

        locked = false;
    }
}
