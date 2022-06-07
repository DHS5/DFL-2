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
    
    // ### Properties ###
    public float YMouseSensitivity
    {
        get { return main.SettingsManager.YMouseSensitivity; }
        set { player.fpsCamera.YMS = value; }
    }
    public float YSmoothRotation
    {
        get { return main.SettingsManager.YSmoothRotation; }
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
            }
            else if (value == ViewType.TPS)
            {
                player.TPPlayer.SetActive(true);
                player.FPPlayer.SetActive(false);

                player.fpsCamera.enabled = false;
            }
        }
    }



    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    public void PreparePlayer()
    {
        playerObject = Instantiate(playerPrefabs[main.GameManager.gameData.playerIndex], new Vector3(0, 0, -25), Quaternion.identity);
        player = playerObject.GetComponent<Player>();

        player.gameManager = main.GameManager;
        player.playerManager = this;


        ViewType = main.DataManager.gameplayData.viewType;
    }

    public void StartPlayer()
    {
        player.gameplay.freeze = false; // Unfreezes the player
        player.gameplay.isChasable = true; // Makes the player chasable
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

    public void SprintUIAnimation()
    {
        StartCoroutine(main.GameUIManager.AccBarAnim(player.controller.accelerationTime, player.controller.accelerationRestTime));
    }
}
