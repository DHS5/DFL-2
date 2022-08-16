using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StadiumCard : TerrainCard
{
    public StadiumCardSO stadiumCardSO { get { return cardSO as StadiumCardSO; } }

    protected override void Start()
    {
        base.Start();
    }
}
