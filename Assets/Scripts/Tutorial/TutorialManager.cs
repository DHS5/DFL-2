using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private MainManager main;
    private DataManager dataManager;

    [SerializeField] private GameObject tutorialPlayer;


    private bool step0 = true;
    private bool step1 = false;
    private bool step2 = false;
    private bool step3 = false;


    [SerializeField] private float step1Time;
    [SerializeField] private float step2Time;
    [SerializeField] private float step3Time;
    [SerializeField] private float step4Time;
    [SerializeField] private float step5Time;

    [SerializeField] private GameObject step1Text;
    [SerializeField] private GameObject step2Text;
    [SerializeField] private GameObject step3Text;
    [SerializeField] private GameObject step4Text;


    private float side = 0f;

    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = FindObjectOfType<DataManager>();

        if (dataManager.gameData.gameMode == GameMode.TUTORIAL)
        {
            dataManager.gameData.gameMode = GameMode.DRILL;
            dataManager.gameData.gameDrill = GameDrill.PRACTICE;
            dataManager.gameData.gameDifficulty = GameDifficulty.EASY;
            dataManager.gameData.gameOptions.Clear();
            dataManager.gameData.gameWheather = GameWheather.SUN;
            dataManager.gameData.player = tutorialPlayer;
            dataManager.gameData.stadiumIndex = 0;
        }
    }

    private void Start()
    {
        main.GameUIManager.SetScreen(GameScreen.TUTO, true);
        
        Invoke(nameof(Step1), step1Time);
    }

    private void Update()
    {
        if (step0)
        {
            main.PlayerManager.player.controller.SnapAcc();
            main.PlayerManager.player.controller.SnapDir();
            main.PlayerManager.player.controller.CanAccelerate = false;
        }

        if (step1)
        {
            main.PlayerManager.player.controller.SnapAcc();

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                main.PlayerManager.StopPlayer();
            }
            else if (Input.GetAxisRaw("Horizontal") * side >= 0)
            {
                side = Input.GetAxisRaw("Horizontal");
                main.PlayerManager.StartPlayer();
                main.PlayerManager.player.controller.FullDir(side);
            }

            if (Mathf.Abs(main.PlayerManager.player.transform.position.x) > 2.5f)
            {
                step1 = false;
                step1Text.SetActive(false);
                Invoke(nameof(Step2), step2Time);
            }
        }

        else if (step2)
        {
            main.PlayerManager.player.controller.SnapDir();

            if (Input.GetAxisRaw("Vertical") >= 0)
            {
                main.PlayerManager.StopPlayer();
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                main.PlayerManager.StartPlayer();

                step2 = false;
                step2Text.SetActive(false);
                Invoke(nameof(Step3), step3Time);
            }
        }

        else if (step3)
        {
            main.PlayerManager.player.controller.SnapDir();
            main.PlayerManager.player.controller.CanAccelerate = true;

            if (Input.GetAxisRaw("Vertical") <= 0)
            {
                main.PlayerManager.StopPlayer();
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                main.PlayerManager.StartPlayer();

                step3 = false;
                step3Text.SetActive(false);
                Invoke(nameof(Step4), step4Time);
            }
        }
    }


    private void Step1()
    {
        step0 = false;
        step1 = true;
        step1Text.SetActive(true);

        main.PlayerManager.player.transform.position = new Vector3(0, 0, main.PlayerManager.player.transform.position.z);
    }

    private void Step2()
    {
        step2 = true;
        step2Text.SetActive(true);
    }

    private void Step3()
    {
        step3 = true;
        step3Text.SetActive(true);
    }

    private void Step4()
    {
        step4Text.SetActive(true);

        main.PlayerManager.StopPlayer();

        Invoke(nameof(Step5), step5Time);
    }

    private void Step5()
    {
        step4Text.SetActive(false);

        main.PlayerManager.StartPlayer();
    }
}
