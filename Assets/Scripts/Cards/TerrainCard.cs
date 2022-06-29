using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TerrainCard : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Sprite sprite;

    public GameObject prefab;


    protected virtual void Awake()
    {
        image.sprite = sprite;
    }
}
