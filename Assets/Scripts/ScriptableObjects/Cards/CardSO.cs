using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class CardSO : ScriptableObject
{
    public string Title;
    public Sprite mainSprite;
    public Sprite shopSprite;
    public GameObject prefab;
    public int price;

    [Range(0, 10)] public int physical;
    [Range(0, 10)] public int handling;
    [Range(0, 5)] public int skills;

    public string position;
}
