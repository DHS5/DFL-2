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
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI popupText;

    protected int price;

    public virtual void GenerateCard(GameObject prefab, string title, Sprite sprite, int _price)
    {
        text.text = title;
        image.sprite = sprite;
        price = _price;
        buttonText.text = price.ToString();
        popupText.text = "Are you sure you want to buy " + title + " ?";
    }

    public virtual void Buy()
    {
        DataManager dataManager = DataManager.InstanceDataManager;
        if (dataManager != null)
        {
            if (dataManager.progressData.coins >= price)
            {
                Debug.Log("Buy");
            }
            else
            {
                Debug.Log("You don't have enough coins to buy " + text.text);
            }
        }
    }
}
