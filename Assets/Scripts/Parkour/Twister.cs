using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : MonoBehaviour
{
    public float rotationSpeed;

    void LateUpdate()
    {
        gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
