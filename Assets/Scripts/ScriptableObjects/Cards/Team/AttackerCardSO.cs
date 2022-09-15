using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum AttackerEnum { NULL, FRONT1, BACK1, RSIDE1, LSIDE1, FRONT2 }


public abstract class AttackerCardSO : ShopCardSO
{
    [Header("Attacker card specifics")]
    public AttackerEnum attacker;

    public AttackerAttributesSO attribute;
    public override object cardObject { get { return attacker; } }

    public abstract string Position { get; }

    public Sprite largeSprite;
}
