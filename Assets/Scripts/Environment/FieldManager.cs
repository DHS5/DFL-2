using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Contains the different types of field and a method to generates one given a difficulty
/// </summary>
public class FieldManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    [SerializeField] private MainManager main;


    [Header("Nav Mesh Surface")]
    [Tooltip("Nav Mesh Surface of the current field")]
    [SerializeField] private NavMeshSurface surface;


    [Header("Training stadium (1v1)")]
    [SerializeField] private GameObject trainingStadium;



    [Tooltip("Current stadium game object")]
    private GameObject stadiumObject;

    public Field field { get; private set; }
    public Stadium stadium { get; private set; }




    // ### Functions ###

    /// <summary>
    /// Generates a random field
    /// </summary>
    public void GenerateField()
    {
        // ### Creation of the field and the stadium
        // ## Instantiation of the prefabs
        bool isOneVOne = main.GameManager.gameData.gameMode == GameMode.DRILL && main.GameManager.gameData.gameDrill == GameDrill.ONEVONE;
        stadiumObject = Instantiate(isOneVOne ? trainingStadium : main.GameManager.gameData.stadium, Vector3.zero, Quaternion.identity);

        field = stadiumObject.GetComponentInChildren<Field>();
        stadium = stadiumObject.GetComponentInChildren<Stadium>();

        // ## Gets random field's materials
        field.CreateField(main.GameManager.gameData);

        // ## Actualization of the Nav Mesh
        surface.BuildNavMesh();

        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE) stadium.SwitchLightsOff();
    }

    public void ActuField()
    {
        field.ActivateFieldLimits();
    }

    /// <summary>
    /// Destroys the former field and stadium
    /// </summary>
    public void DestroyField()
    {
        // Destroys the stadium and field
        Destroy(stadiumObject);
    }

    /// <summary>
    /// Called when the game is over
    /// Activates the stadium camera and the lose audios
    /// </summary>
    public void GameOver()
    {
        // Activates the stadium camera
        stadium.stadiumCamera.gameObject.SetActive(true);
    }
}
