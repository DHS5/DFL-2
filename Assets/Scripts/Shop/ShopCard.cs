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

    protected CardSO cardSO;
    protected int price;

    public virtual void GenerateCard(CardSO _cardSO)
    {
        cardSO = _cardSO;

        text.text = cardSO.Title;
        image.sprite = cardSO.mainSprite;
        price = cardSO.price;
        buttonText.text = price.ToString();
        popupText.text = "Are you sure you want to buy " + cardSO.Title + " ?";
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
