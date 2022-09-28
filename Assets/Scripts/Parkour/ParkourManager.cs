using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ParkourManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    private DataManager dataManager;

    public Parkour Parkour { get; private set; }


    [SerializeField] private PlayerInfo parkourPlayer;
    [SerializeField] private GameObject parkourStadium;


    private void Awake()
    {
        main = GetComponent<MainManager>();
        dataManager = FindObjectOfType<DataManager>();

        if (dataManager.gameData.gameMode == GameMode.DRILL && dataManager.gameData.gameDrill == GameDrill.PARKOUR)
        {
            dataManager.gameData.gameDifficulty = GameDifficulty.EASY;
            dataManager.gameData.gameOptions.Clear();
            dataManager.gameData.gameWeather = GameWeather.NIGHT;
            dataManager.gameData.player = parkourPlayer;
            dataManager.gameData.stadium = parkourStadium;
        }
    }


    // ### Functions ###


    public void GenerateParkour()
    {
        Vector3 position = main.FieldManager.field.enterZone.transform.position;

        Parkour = Instantiate(main.GameManager.gameData.parkour.gameObject, position, Quaternion.identity).GetComponent<Parkour>();
    }

    public void Win()
    {
        if (Won((int)Parkour.ParkourNum))
            main.DataManager.inventoryData.coins += Parkour.BaseReward;

        else
        {
            main.DataManager.inventoryData.coins += Parkour.Reward;
            UnlockNextParkour();
        }
    }

    private void UnlockNextParkour()
    {
        List<int> parkours = main.DataManager.inventoryData.parkours.ToList();
        parkours.Add((int)Parkour.ParkourNum + 1);
        main.DataManager.inventoryData.parkours = parkours.ToArray();
    }

    public static bool Won(int parkourNum)
    {
        foreach (int i in DataManager.InstanceDataManager.inventoryData.parkours)
            if (i == parkourNum + 1) return true;

        return false;
    }
}
