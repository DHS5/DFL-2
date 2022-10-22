using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    

    [Tooltip("Player prefab")]
    [SerializeField] private GameObject playerBasePrefab;

    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public Player player;


    private bool flashlight = false;
    
    // ### Properties ###
    public float HeadAngle
    {
        get { return main.DataManager.gameplayData.headAngle; }
        set { player.fPPlayer.fpsCamera.HeadAngle = value; }
    }
    public float YMouseSensitivity
    {
        get { return main.DataManager.gameplayData.yms; }
        set { player.fPPlayer.fpsCamera.YMS = value; }
    }
    public float YSmoothRotation
    {
        get { return main.DataManager.gameplayData.ysr; }
        set { player.fPPlayer.fpsCamera.YSR = value; }
    }
    public ViewType ViewType
    {
        get { return main.DataManager.gameplayData.viewType; }
        set
        {
            if (value == ViewType.FPS)
            {
                player.fPPlayer.gameObject.SetActive(true);
                player.tPPlayer.gameObject.SetActive(false);

                player.fPPlayer.fpsCamera.enabled = true;
                player.tPPlayer.tpsCamera.enabled = false;

                player.activeBody = player.fPPlayer.gameObject;
            }
            else if (value == ViewType.TPS)
            {
                player.tPPlayer.gameObject.SetActive(true);
                player.fPPlayer.gameObject.SetActive(false);

                player.tPPlayer.tpsCamera.enabled = true;
                player.fPPlayer.fpsCamera.enabled = false;

                player.activeBody = player.tPPlayer.gameObject;
            }

            main.GameManager.ViewChange();
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

    public bool FlashlightActive
    {
        get { return flashlight; }
        set 
        { 
            flashlight = value;
            player.fPPlayer.flashlight.gameObject.SetActive(value);
            player.fPPlayer.flashlight.difficulty = main.GameManager.gameData.gameDifficulty;
            player.tPPlayer.flashlight.gameObject.SetActive(value);
            player.tPPlayer.flashlight.difficulty = main.GameManager.gameData.gameDifficulty;
        }
    }


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    public void PreparePlayer()
    {
        playerObject = Instantiate(playerBasePrefab, Vector3.zero, Quaternion.identity);
        player = playerObject.GetComponentInChildren<Player>();
        player.CreatePlayer(main.GameManager.gameData.player);

        player.gameManager = main.GameManager;
        player.fieldManager = main.FieldManager;
        player.playerManager = this;


        ViewType = main.DataManager.gameplayData.viewType;
        FlashlightActive = main.GameManager.gameData.gameMode == GameMode.ZOMBIE;

    }

    public void PositionPlayer()
    {
        playerObject.transform.position = main.FieldManager.stadium.SpawnPosition.transform.position;
    }

    public void StartPlayer()
    {
        player.gameplay.freeze = false; // Unfreezes the player
        //fpsCamera.LockCursor(); // Locks the cursor

        player.fPPlayer.animator.enabled = true;
        player.tPPlayer.animator.enabled = true;
    }

    public void StopPlayer()
    {
        player.gameplay.freeze = true; // Freezes the player

        player.fPPlayer.animator.enabled = false;
        player.tPPlayer.animator.enabled = false;
    }

    public void DeadPlayer()
    {
        main.GameManager.GameOver = true; // Game Over

        player.gameplay.freeze = true; // Player freezes

        player.effects.Rain(false, 0); // Stops eventual rain
    }

    public void WinPlayer()
    {
        main.GameManager.Win();

        player.gameplay.freeze = true; // Player freezes
    }


    // UI functions

    public void SprintUIAnimation()
    {
        if (!player.controller.Sprinting)
            StartCoroutine(main.GameUIManager.AccBarAnim(player.controller.playerAtt.accelerationTime, player.controller.playerAtt.accelerationRestTime));
    }

    public void JumpUIAnimation(float cost)
    {
        main.GameUIManager.jumpBar.Jump(cost);
    }

    public void UIModifyLife(bool state, int lifeNumber)
    {
        main.GameUIManager.ModifyLife(state, lifeNumber);
    }


    // Other

    public int EnemyNumber(float radius, out bool zombie)
    {
        zombie = main.GameManager.gameData.gameMode == GameMode.ZOMBIE;

        int total = 0;
        foreach (Enemy e in main.EnemiesManager.enemies)
        {
            if (e.rawDistance <= radius && e.zDistance > -1) total++;
        }

        return total;
    }
}
