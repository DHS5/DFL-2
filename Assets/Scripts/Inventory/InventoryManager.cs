using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    private MenuMainManager main;


    private List<PlayerEnum> players = new();
    private List<StadiumEnum> stadiums = new();
    private List<AttackerEnum> attackers = new();
    private List<WeaponEnum> weapons = new();
    private List<ParkourEnum> parkours = new();


    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }


    public void ActuInventory()
    {
        GetInventory();
        main.CardManager.GetCards();
        main.ShopManager.GenerateShopButtons();
    }


    public bool IsInInventory(object obj)
    {
        if (obj == null) return true;
        else if (obj.GetType() == typeof(PlayerEnum))
            return players.Contains((PlayerEnum)obj);
        else if (obj.GetType() == typeof(StadiumEnum))
            return stadiums.Contains((StadiumEnum)obj);
        else if (obj.GetType() == typeof(AttackerEnum))
            return attackers.Contains((AttackerEnum)obj);
        else if (obj.GetType() == typeof(WeaponEnum))
            return weapons.Contains((WeaponEnum)obj);
        else if (obj.GetType() == typeof(ParkourEnum))
            return parkours.Contains((ParkourEnum)obj);
        else return false;
    }

    public void AddToInventory(object obj)
    {
        if (obj.GetType() == typeof(PlayerEnum))
            players.Add((PlayerEnum)obj);
        else if (obj.GetType() == typeof(StadiumEnum))
            stadiums.Add((StadiumEnum)obj);
        else if (obj.GetType() == typeof(AttackerEnum))
            attackers.Add((AttackerEnum)obj);
        else if (obj.GetType() == typeof(WeaponEnum))
            weapons.Add((WeaponEnum)obj);
        else if (obj.GetType() == typeof(ParkourEnum))
            parkours.Add((ParkourEnum)obj);

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
        main.DataManager.inventoryData.players = inv;
        // Stadiums
        inv = new int[stadiums.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)stadiums[i];
        }
        main.DataManager.inventoryData.stadiums = inv;
        // Attackers
        inv = new int[attackers.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)attackers[i];
        }
        main.DataManager.inventoryData.attackers = inv;
        // Weapons
        inv = new int[weapons.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)weapons[i];
        }
        main.DataManager.inventoryData.weapons = inv;
        // Parkours
        inv = new int[parkours.Count];
        for (int i = 0; i < inv.Length; i++)
        {
            inv[i] = (int)parkours[i];
        }
        main.DataManager.inventoryData.parkours = inv;

        main.DataManager.SaveDatas(false);
    }


    private void GetInventory()
    {
        ClearInventory();

        // Players
        foreach (int i in main.DataManager.inventoryData.players)
            players.Add((PlayerEnum) i);
        // Stadiums
        foreach (int i in main.DataManager.inventoryData.stadiums)
            stadiums.Add((StadiumEnum) i);
        // Attackers
        foreach (int i in main.DataManager.inventoryData.attackers)
            attackers.Add((AttackerEnum) i);
        // Weapons
        foreach (int i in main.DataManager.inventoryData.weapons)
            weapons.Add((WeaponEnum) i);
        GetWeaponsFromInventory();
        // Parkours
        foreach (int i in main.DataManager.inventoryData.parkours)
            parkours.Add((ParkourEnum) i);
    }

    private void ClearInventory()
    {
        players.Clear();
        stadiums.Clear();
        attackers.Clear();
        weapons.Clear();
        parkours.Clear();
    }

    private void GetWeaponsFromInventory()
    {
        main.DataManager.gameData.weapons = new List<GameObject>();

        foreach (WeaponCardSO wCard in main.DataManager.cardsContainer.weaponCards)
        {
            if (IsInInventory(wCard.cardObject))
                main.DataManager.gameData.weapons.Add(wCard.prefab);
        }
    }
}
