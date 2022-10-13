using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SIDE { LEFT = -1, RIGHT = 1 }
public abstract class SideAttAttributesSO : AttackerAttributesSO
{
    public virtual SIDE Side { get; }

    public float blockDistance;
    public float blockAngle;
    public float blockLate;
}


