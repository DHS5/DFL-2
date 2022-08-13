using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StadiumCard : TerrainCard
{
    [HideInInspector] public StadiumCardSO stadiumCardSO;

    protected override void Start()
    {
        base.Start();

        stadiumCardSO = cardSO as StadiumCardSO;
    }
}
