using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    

    [Tooltip("List of player prefabs")]
    [SerializeField] private GameObject[] playerPrefabs;

    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public Player player;


    [Tooltip("Player's start position")]
    [SerializeField] private Vector3 startPosition;

    
    // ### Properties ###
    public float YMouseSensitivity
    {
        get { return main.DataManager.gameplayData.yms; }
        set { player.fpsCamera.YMS = value; }
    }
    public float YSmoothRotation
    {
        get { return main.DataManager.gameplayData.ysr; }
        set { player.fpsCamera.YSR = value; }
    }
    public ViewType ViewType
    {
        set
        {
            if (value == ViewType.FPS)
            {
                player.FPPlayer.SetActive(true);
                player.TPPlayer.SetActive(false);

                player.fpsCamera.enabled = true;

                player.activeBody = player.FPPlayer;
            }
            else if (value == ViewType.TPS)
            {
                player.TPPlayer.SetActive(true);
                player.FPPlayer.SetActive(false);

                player.fpsCamera.enabled = false;

                player.activeBody = player.TPPlayer;
            }
        }
    }
    public int FpCameraPos
    {
        get { return main.DataManager.gameplayData.fpCameraPos; }
        set { main.DataManager.gameplayData.fpCameraPos = value; }
    }
    public int TpCameraPos
    {
        get { return main.DataManager.gameplayData.tpCameraPos; }
        set { main.DataManager.gameplayData.tpCameraPos = value; }
    }



    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    public void PreparePlayer()
    {
        playerObject = Instantiate(main.GameManager.gameData.player, startPosition, Quaternion.identity);
        player = playerObject.GetComponent<Player>();

        player.gameManager = main.GameManager;
        player.playerManager = this;


        ViewType = main.DataManager.gameplayData.viewType;
    }

    public void StartPlayer()
    {
        player.gameplay.freeze = false; // Unfreezes the player
        //fpsCamera.LockCursor(); // Locks the cursor

        player.fpAnimator.enabled = true;
        player.tpAnimator.enabled = true;
    }

    public void StopPlayer()
    {
        player.gameplay.freeze = true; // Freezes the player

        player.fpAnimator.enabled = false;
        player.tpAnimator.enabled = false;
    }

    public void DeadPlayer()
    {
        player.gameplay.freeze = true; // Player freezes
        // Player animator stops
        //playerRunAnimator.SetTrigger("Dead");
    }


    // UI functions

    public void SprintUIAnimation()
    {
        if (!player.controller.Sprinting)
            StartCoroutine(main.GameUIManager.AccBarAnim(player.controller.accelerationTime, player.controller.accelerationRestTime));
    }

    public void UIModifyLife(bool state, int lifeNumber)
    {
        main.GameUIManager.ModifyLife(state, lifeNumber);
    }
}
