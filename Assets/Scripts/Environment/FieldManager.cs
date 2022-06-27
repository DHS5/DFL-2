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
    private MainManager main;


    [Header("Nav Mesh Surface")]
    [Tooltip("Nav Mesh Surface of the current field")]
    [SerializeField] private NavMeshSurface surface;

    [Header("Stadium prefabs")]
    [Tooltip("Prefab list of the stadiums")]
    [SerializeField] private GameObject[] stadiumPrefabs;


    [Tooltip("Current stadium game object")]
    private GameObject stadiumObject;

    public Field field { get; private set; }
    public Stadium stadium { get; private set; }


    [Tooltip("Vector 3 containing the position of the actual field")]
    private Vector3 fieldPosition = new Vector3(0, 0, -289);
    [Tooltip("Vector 3 giving the distance between the position of 2 fields")]
    private Vector3 fieldDistance = new Vector3(0,0,289);




    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###

    /// <summary>
    /// Generates a random field
    /// </summary>
    public void GenerateField()
    {
        // Actualizes the public field position
        fieldPosition += fieldDistance;

        // ### Creation of the field and the stadium
        // ## Instantiation of the prefabs
        stadiumObject = Instantiate(stadiumPrefabs[main.GameManager.gameData.stadiumIndex], fieldPosition, Quaternion.identity);

        field = stadiumObject.GetComponentInChildren<Field>();
        stadium = stadiumObject.GetComponentInChildren<Stadium>();

        // ## Gets random field's materials
        field.CreateField();

        // ## Actualization of the Nav Mesh
        surface.BuildNavMesh();
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
        // Activates the lose audios
        //if (main.GameManager.gameData.gameMode != GameMode.ZOMBIE)
        //    stadium.OuuhAudio();
        //stadium.StopAmbianceAudios();
        //// Calls the booh audios
        //if (main.GameManager.gameData.gameMode != GameMode.ZOMBIE)
        //    stadium.BoohAudio();
    }
}
