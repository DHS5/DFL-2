using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TerrainCard : Card
{
    public override bool InfoActive { get => false; set => infoToggle = null; }
}
