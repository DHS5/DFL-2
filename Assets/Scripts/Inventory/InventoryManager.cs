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


    private void Awake()
    {
        main = GetComponent<MenuMainManager>();
    }


    public void ActuInventory()
    {
        GetInventory();
        main.MenuUIManager.GetCards();
        main.ShopManager.GenerateShopButtons();
    }


    public bool IsInInventory(object obj)
    {
        if (obj.GetType() == typeof(PlayerEnum))
            return players.Contains((PlayerEnum)obj);
        else if (obj.GetType() == typeof(StadiumEnum))
            return stadiums.Contains((StadiumEnum)obj);
        else if (obj.GetType() == typeof(AttackerEnum))
            return attackers.Contains((AttackerEnum)obj);
        else if (obj.GetType() == typeof(WeaponEnum))
            return weapons.Contains((WeaponEnum)obj);
        else if (obj.GetType() == typeof(bool))
            return true;
        else return false;
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
    }


    private void GetInventory()
    {
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
        {
            weapons.Add((WeaponEnum)i);
        }
        GetWeaponsFromInventory();
    }

    private void GetWeaponsFromInventory()
    {
        main.DataManager.gameData.weapons = new List<GameObject>();

        foreach (CardSO wCard in main.DataManager.cardsContainer.weaponCards)
        {
            if (IsInInventory(wCard.type.GetObject()))
                main.DataManager.gameData.weapons.Add(wCard.prefab);
        }
    }
}
