using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private MainManager main;
    private DataManager dataManager;

    [SerializeField] private Tutorial[] tutorials;

    private Tutorial activeTuto;

    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = FindObjectOfType<DataManager>();

        activeTuto = tutorials[dataManager.gameData.tutoNumber];

        if (dataManager.gameData.gameMode == GameMode.TUTORIAL)
        {
            activeTuto.gameObject.SetActive(true);

            dataManager.gameData = activeTuto.gameData;

            main.GameUIManager.SetScreen(GameScreen.TUTO, true);
        }
    }

    private void Start()
    {
        main.FieldManager.field.entryGoalpost.SetActive(activeTuto.goalPostActive);
    }
}
