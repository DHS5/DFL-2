using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private MainManager main;
    private DataManager dataManager;

    [SerializeField] private PlayerInfo tutorialPlayer;
    [SerializeField] private GameObject tutorialStadium;

    [Space]
    [SerializeField] private GameObject tutoContainer;


    private TutorialPopup[] tutos;


    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = FindObjectOfType<DataManager>();

        if (dataManager.gameData.gameMode == GameMode.TUTORIAL)
        {
            dataManager.gameData.gameDrill = GameDrill.PRACTICE;
            dataManager.gameData.gameDifficulty = GameDifficulty.ROOKIE;
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
}
