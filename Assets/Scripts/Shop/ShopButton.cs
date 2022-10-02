using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [Header("UI components")]
    [SerializeField] private Image picture;
    [SerializeField] private TextMeshProUGUI nameText;


    private ShopCardSO cardSO;

    private ShopCard shopCard;

    private LockerRoom lockerRoom;

    [HideInInspector] public bool buyable;
    [HideInInspector] public bool enoughMoney;

    private bool isPlayer = false;


    public void GetCard(ShopCardSO _cardSO, ShopCard _shopCard, bool _buyable, bool _enoughMoney)
    {
        cardSO = _cardSO;
        shopCard = _shopCard;
        buyable = _buyable;
        picture.sprite = cardSO.mainSprite;
        if (nameText != null) nameText.text = cardSO.Title;
        enoughMoney = _enoughMoney;
    }

    public void GetCard(ShopCardSO _cardSO, ShopCard _shopCard, bool _buyable, bool _enoughMoney, LockerRoom locker)
    {
        GetCard(_cardSO, _shopCard, _buyable, _enoughMoney);
        lockerRoom = locker;
        isPlayer = true;
    }


    /// <summary>
    /// Gives the ShopCard the cardSO and generates it
    /// </summary>
    /// <param name="g">Parent of the shop card</param>
    public void ApplyOnShopCard()
    {
        if (shopCard != null)
        {
            shopCard.GenerateCard(cardSO, this, buyable, enoughMoney);
            if (isPlayer) lockerRoom.ApplyPlayerInfo(cardSO as PlayerCardSO);
        }
    }
}
