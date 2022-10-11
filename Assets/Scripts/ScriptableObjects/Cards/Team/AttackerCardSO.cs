using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum AttackerEnum { NULL, FRONT1, BACK1, RSIDE1, LSIDE1, FRONT2, BACK2, RSIDE2, LSIDE2, FRONT3, BACK3, RSIDE3, LSIDE3, 
    FRONT4, BACK4, RSIDE4, LSIDE4, FRONT5, BACK5, RSIDE5, LSIDE5, FRONT6, BACK6, RSIDE6, LSIDE6, FRONT7, BACK7, RSIDE7, LSIDE7, 
    FRONT8, BACK8, RSIDE8, LSIDE8, FRONT9, BACK9, RSIDE9, LSIDE9, FRONT10, BACK10, RSIDE10, LSIDE10, FRONT11, BACK11, RSIDE11, LSIDE11, }


public abstract class AttackerCardSO : ShopCardSO
{
    [Header("Attacker card specifics")]
    public AttackerEnum attacker;

    public AttackerAttributesSO attribute;
    public override object cardObject { get { return attacker; } }

    public abstract string Position { get; }

    public Sprite largeSprite;

    [Header("Locker room attributes")]
    public Mesh mesh;
    public Avatar avatar;
}
