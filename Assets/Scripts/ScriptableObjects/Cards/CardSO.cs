using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardSO : ScriptableObject
{
    public ItemType type;
    [Space]
    public string Title;
    public Sprite mainSprite;
    public Sprite shopSprite;
    public GameObject prefab;
    public int price;
    [Space]
    [Range(0, 10)] public int physical;
    [Range(0, 10)] public int handling;
    [Range(0, 5)] public int skills;
    [Space]
    public string position;
}
