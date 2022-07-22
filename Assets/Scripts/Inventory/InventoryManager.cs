using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    private DataManager dataManager;
    private MenuUIManager menuUIManager;
    private ShopManager shopManager;


    private List<PlayerEnum> players = new();
    private List<StadiumEnum> stadiums = new();
    private List<AttackerEnum> attackers = new();
    private List<WeaponEnum> weapons = new();


    private void Start()
    {
        dataManager = DataManager.InstanceDataManager;
        menuUIManager = GetComponent<MenuUIManager>();
        shopManager = FindObjectOfType<ShopManager>();

        Prepare();
        ActuInventory();
    }

    private void Prepare()
    {
        menuUIManager.GetManagers();
        shopManager.GetManagers();
    }

    private void ActuInventory()
    {
        GetInventory();
        menuUIManager.GetCards();
        shopManager.GenerateShopButtons();
    }


    public bool IsInInventory(object obj)
    {
        if (obj.GetType() == typeof(PlayerEnum))
            return players.Contains((PlayerEnum)obj);
        else if (obj.GetType() == typeof(StadiumEnum))
            return stadiums.Contains((StadiumEnum)obj);
        else if (obj.GetType() == typeof(AttackerEnum))
            return attackers.Contains((AttackerEnum)obj);
        else return weapons.Contains((WeaponEnum)obj);
    }

    public void AddToInventory(object obj)
    {
        if (obj.GetType() == typeof(PlayerEnum))
            players.Add((PlayerEnum)obj);
        else if (obj.GetType() == typeof(StadiumEnum))
            stadiums.Add((StadiumEnum)obj);
        else if (obj.GetType() == typeof(StadiumEnum))
            attackers.Add((AttackerEnum)obj);
        else weapons.Add((WeaponEnum)obj);

        SaveInventory();
        ActuInventory();
    }


    private void SaveInventory()
    {
        // Players
        int[] inv = new int[players.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)players[i];
        }
        dataManager.inventoryData.players = inv;
        // Stadiums
        inv = new int[stadiums.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)stadiums[i];
        }
        dataManager.inventoryData.stadiums = inv;
        // Attackers
        inv = new int[attackers.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)attackers[i];
        }
        dataManager.inventoryData.attackers = inv;
        // Weapons
        inv = new int[weapons.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)weapons[i];
        }
        dataManager.inventoryData.weapons = inv;
    }


    private void GetInventory()
    {
        // Players
        foreach (int i in dataManager.inventoryData.players)
            players.Add((PlayerEnum) i);
        // Stadiums
        foreach (int i in dataManager.inventoryData.stadiums)
            stadiums.Add((StadiumEnum) i);
        // Attackers
        foreach (int i in dataManager.inventoryData.attackers)
            attackers.Add((AttackerEnum) i);
        // Weapons
        foreach (int i in dataManager.inventoryData.weapons)
        {
            weapons.Add((WeaponEnum)i);
        }
        GetWeaponsFromInventory();
    }

    private void GetWeaponsFromInventory()
    {
        dataManager.gameData.weapons = new List<GameObject>();

        foreach (CardSO wCard in dataManager.cardsContainer.weaponCards)
        {
            if (IsInInventory(wCard.type.GetObject()))
                dataManager.gameData.weapons.Add(wCard.prefab);
        }
    }
}
