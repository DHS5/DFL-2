using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum WeaponEnum { NULL, KNIFE, M1911, AK74, M16, SHOTGUN, UZI }

[CreateAssetMenu(fileName = "WeaponCard", menuName = "ScriptableObjects/Card/WeaponCard", order = 1)]
public class WeaponCardSO : ShopCardSO
{
    [Header("Weapon card specifics")]
    public WeaponEnum weapon;

    public GameObject prefab;

    public Sprite bulletSprite;

    public override object cardObject { get { return weapon; } }
}
