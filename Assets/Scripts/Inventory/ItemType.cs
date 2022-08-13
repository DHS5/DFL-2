using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class ItemType
{
    public PlayerEnum player;
    public StadiumEnum stadium;
    public AttackerEnum attacker;
    public WeaponEnum weapon;
    public bool isParkour;
    public bool isEnemy;


    public object GetObject()
    {
        if (player != PlayerEnum.NULL) return player;
        else if (stadium != StadiumEnum.NULL) return stadium;
        else if (attacker != AttackerEnum.NULL) return attacker;
        else if (weapon != WeaponEnum.NULL) return weapon;
        else if (isParkour || isEnemy) return isParkour;
        else return null;
    }
}