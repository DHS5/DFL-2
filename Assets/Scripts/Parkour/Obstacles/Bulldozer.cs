using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulldozer : MonoBehaviour
{
    public GameObject axis;
    public GameObject bowl;

    [Tooltip("Height between low and high position")]
    public float range;

    public float rotationSpeed;
    [Space]
    public bool rotX;
    public bool rotY;
    public bool rotZ;

    private float startHeight;
    private int right;
    float rotSpeed;

    private void Start()
    {
        startHeight = bowl.transform.position.y;
        right = 1;
    }

    void LateUpdate()
    {
        if (bowl.transform.position.y >= startHeight + range) right = -right;

        rotSpeed = right * rotationSpeed * Time.deltaTime;

        axis.transform.Rotate(rotX ? rotSpeed : 0, rotY ? rotSpeed : 0 , rotZ ? rotSpeed : 0);
    }
}
