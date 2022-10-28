using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : MonoBehaviour
{
    public float rotationSpeed;
    [Space]
    public bool rotX;
    public bool rotY;
    public bool rotZ;

    void LateUpdate()
    {
        float rotSpeed = rotationSpeed * Time.deltaTime;
        gameObject.transform.Rotate(rotX ? rotSpeed : 0, rotY ? rotSpeed : 0 , rotZ ? rotSpeed : 0);
    }
}
