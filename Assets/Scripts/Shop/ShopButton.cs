using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButton : MonoBehaviour
{
    private ShopManager shopManager;


    [Header("UI components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Fields")]
    [Tooltip("Text to display under the button")]
    [SerializeField] private string title;
    [Tooltip("Sprite to display on the button")]
    [SerializeField] private Sprite sprite;
    [Tooltip("Sprite to display on the shop card")]
    [SerializeField] private Sprite shopCardSprite;
    [Tooltip("Shop card prefab")]
    [SerializeField] private GameObject shopCardPrefab;
    [Tooltip("Shop object prefab (object to buy)")]
    [SerializeField] private GameObject shopObjectPrefab;


    private ShopCard shopCard;


    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();

        text.text = title;
        button.image.sprite = sprite;
    }


    /// <summary>
    /// Instantiate a shop card and passes it the corresponding prefab
    /// </summary>
    /// <param name="g">Parent of the shop card</param>
    public void InstantiateShopCard()
    {
        if (shopCard == null)
        {
            shopCard = Instantiate(shopCardPrefab, shopManager.OpenShop().transform).GetComponent<ShopCard>();
            shopCard.GenerateCard(shopObjectPrefab, title, shopCardSprite);
        }
        else
        {
            shopCard.gameObject.SetActive(true);
            shopManager.OpenShop();
        }
    }
}
