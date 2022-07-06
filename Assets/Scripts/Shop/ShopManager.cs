using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopBackground;
    [SerializeField] private GameObject shopCardContainer;




    public GameObject OpenShop()
    {
        shopBackground.SetActive(true);

        return shopCardContainer;
    }

    public void DeactivateShopCards()
    {
        shopBackground.SetActive(false);

        foreach(ShopCard sc in shopCardContainer.GetComponentsInChildren<ShopCard>())
            sc.gameObject.SetActive(false);
    }
}
