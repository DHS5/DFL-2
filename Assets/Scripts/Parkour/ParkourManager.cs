using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    private GameObject parkour;


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    public void GenerateParkour()
    {
        Vector3 position = main.FieldManager.field.fieldZone.transform.position;

        parkour = Instantiate(main.GameManager.gameData.parkour, position, Quaternion.identity);
    }
}
