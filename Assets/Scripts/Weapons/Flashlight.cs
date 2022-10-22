using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct FlashlightAttribute
{
    public float intensity;
    public float range;
    public float angle;
}

public class Flashlight : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Light spotlight;

    [Header("Caracteristics")]
    [SerializeField] private FlashlightAttribute att;


    private FlashlightAttribute[] flashlightAttributes =
    {
        new FlashlightAttribute { intensity = 75, range = 25, angle = 120 },
        new FlashlightAttribute { intensity = 65, range = 20, angle = 110 },
        new FlashlightAttribute { intensity = 55, range = 17.5f, angle = 95 },
        new FlashlightAttribute { intensity = 50, range = 15, angle = 85 },
        new FlashlightAttribute { intensity = 45, range = 12.5f, angle = 70 }
    };

    private bool gotAttribute = false;

    // ### Properties ###

    public FlashlightAttribute Attribute
    {
        set
        {
            spotlight.range = value.range;
            spotlight.intensity = value.intensity;
            spotlight.spotAngle = value.angle;
            spotlight.innerSpotAngle = value.angle - value.angle / 5;
            gotAttribute = true;
        }
    }
    public GameDifficulty difficulty
    {
        set
        {
            Attribute = flashlightAttributes[(int)value];
        }
    }

    private void Start()
    {
        if (!gotAttribute) Attribute = att;
    }
}
