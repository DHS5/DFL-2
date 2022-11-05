using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum PlayerEnum 
{ 
    NULL, STEVE, KITTLE, OBJ, LAMAR, BLZ, CHEETAH, TAE, NOAH, KINGHENRY, JJ, AARON, AMON, ANT, ZEKE, KUP, DEVONTA, JAMARR, SAQUAD, KAMARU, 
    CMC, HOP, KELCE, DK, TAYLOR, KEENAN, DIGZ, NAJEE, EVANS, McLAW, CHUBB, MOSTERT, PITTS, COOKS, HUNTER, ENGRAM, FIELDS, RUSSEL, ELIJAH, MEYERS
}

public enum PlayerClass { ROOKIE, PRO, STAR, ALLPRO, MVP, ICON, LEGEND }


[CreateAssetMenu(fileName = "PlayerCard", menuName = "ScriptableObjects/Card/PlayerCard", order = 1)]
public class PlayerCardSO : ShopCardSO
{
    [Header("Player card specifics")]
    public PlayerEnum player;

    public PlayerInfo playerInfo;
    public PlayerClass playerClass;
    public override object cardObject { get { return player; } }

    [Header("General levels")]
    [Range(0, 10)] public int physical;
    [Range(0, 10)] public int handling;
    [Range(0, 10)] public int skills;

    [Header("Locker Room's attributes")]
    public PlayerPauses playerPause;
    public bool footballActive;


    public override void SetActive()
    {
        DataManager.InstanceDataManager.gameData.player = playerInfo;
    }
}
