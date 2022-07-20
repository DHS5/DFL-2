using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerEnum { NULL, OBJ, LAMAR }
[System.Serializable]
public enum StadiumEnum { NULL, KETTLE, ARROWHEAD }
[System.Serializable]
public enum AttackerEnum { NULL, FRONT1, BACK1, RSIDE1, LSIDE1 }
[System.Serializable]
public enum WeaponEnum { NULL, KNIFE, GUN }


[System.Serializable]
public class ItemType
{
    public PlayerEnum player;
    public StadiumEnum stadium;
    public AttackerEnum attacker;
    public WeaponEnum weapon;

    
    public object GetObject()
    {
        if (player != PlayerEnum.NULL) return player;
        else if (stadium != StadiumEnum.NULL) return stadium;
        else if (attacker != AttackerEnum.NULL) return attacker;
        else return weapon;
    }
}