using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopCard : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button buyButton;

    public virtual void GenerateCard(GameObject prefab, string title, Sprite sprite)
    {
        text.text = title;
        image.sprite = sprite;
    }
}
