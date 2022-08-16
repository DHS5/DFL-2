using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;


    public Parkour Parkour { get; private set; }


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }


    // ### Functions ###


    public void GenerateParkour()
    {
        Vector3 position = main.FieldManager.field.enterZone.transform.position;

        Parkour = Instantiate(main.GameManager.gameData.parkour.gameObject, position, Quaternion.identity).GetComponent<Parkour>();
    }
}
