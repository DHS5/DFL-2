using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private MainManager main;
    private DataManager dataManager;

    [SerializeField] private GameObject tutorialPlayer;
    [SerializeField] private GameObject tutorialStadium;


    private bool step0 = true;
    private bool step1 = false;
    private bool step2 = false;
    private bool step3 = false;
    private bool step4 = false;
    private bool step5 = false;

    [Space]
    [SerializeField] private GameObject tutoContainer;
    [Space]
    [SerializeField] private float step1Time;
    [SerializeField] private float step2Time;
    [SerializeField] private float step3Time;
    [SerializeField] private float step4Time;
    [SerializeField] private float step5Time;
    [SerializeField] private float step6Time;
    [SerializeField] private float lastStepTime;
    [Space]
    [SerializeField] private GameObject step1Text;
    [SerializeField] private GameObject step2Text;
    [SerializeField] private GameObject step3Text;
    [SerializeField] private GameObject step4Text;
    [SerializeField] private GameObject step5Text;
    [SerializeField] private GameObject step6Text;


    private TutorialPopup[] tutos;


    private float side = 0f;

    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = FindObjectOfType<DataManager>();

        if (dataManager.gameData.gameMode == GameMode.TUTORIAL)
        {
            dataManager.gameData.gameDrill = GameDrill.PRACTICE;
            dataManager.gameData.gameDifficulty = GameDifficulty.EASY;
            dataManager.gameData.gameOptions.Clear();
            dataManager.gameData.gameWeather = GameWeather.SUN;
            dataManager.gameData.player = tutorialPlayer;
            dataManager.gameData.stadium = tutorialStadium;
        }

        InitTutosArray();
    }

    private void Start()
    {
        main.GameUIManager.SetScreen(GameScreen.TUTO, true);

        StartCoroutine(TutorialCR());

        //Invoke(nameof(Step1), step1Time);

        main.FieldManager.field.entryGoalpost.SetActive(true);
    }


    private void InitTutosArray()
    {
        tutos = tutoContainer.GetComponentsInChildren<TutorialPopup>();
        foreach (TutorialPopup tuto in tutos)
        {
            tuto.gameObject.SetActive(false);
        }
    }


    private IEnumerator TutorialCR()
    {
        for (int i = 0; i < tutos.Length; i++)
        {
            yield return new WaitForSeconds(tutos[i].timeBeforeShowingUp);

            tutos[i].PreCondition();

            yield return new WaitUntil(() => tutos[i].CanPass);

            Destroy(tutos[i].gameObject);
        }
    }


    private void FormerUpdate()
    {
        if (step0)
        {
            main.PlayerManager.player.controller.SnapAcc();
            main.PlayerManager.player.controller.SnapDir();
            main.PlayerManager.player.controller.CanAccelerate = false;
        }
        // Side
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
        // Slow
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
        // Sprint
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
        // Jump
        else if (step4)
        {
            main.PlayerManager.player.controller.SnapDir();
            main.PlayerManager.player.controller.SnapAcc();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                main.PlayerManager.StartPlayer();

                step4 = false;
                step4Text.SetActive(false);
                Invoke(nameof(Step5), step5Time);
            }
        }
        // Slowsiderun
        else if (step5)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") < 0)
            {
                main.PlayerManager.StartPlayer();

                step5 = false;
                step5Text.SetActive(false);
                Invoke(nameof(Step6), step6Time);
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
        step4 = true;
        step4Text.SetActive(true);
        main.PlayerManager.StopPlayer();
        main.PlayerManager.player.controller.CanAccelerate = false;
    }

    private void Step5()
    {
        step5 = true;
        step5Text.SetActive(true);
        main.PlayerManager.StopPlayer();
    }

    private void Step6()
    {
        step6Text.SetActive(true);

        main.PlayerManager.StopPlayer();
        Invoke(nameof(LastStep), lastStepTime);
    }

    private void LastStep()
    {
        step6Text.SetActive(false);
        main.PlayerManager.StartPlayer();
    }
}
