using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Distance between middle and max/min position")]
    public float range;

    [Tooltip("Time to reach max/min position from middle")]
    public float time;

    private Vector3 middlePos;

    public bool ascending;

    private void Start()
    {
        middlePos = transform.position;
    }

    void LateUpdate()
    {
        if (ascending && transform.position.y < middlePos.y + range)
        {
            transform.Translate(0, (range / time) * Time.deltaTime, 0);
        }

        else if (!ascending && transform.position.y > middlePos.y - range)
        {
            transform.Translate(0, -(range / time) * Time.deltaTime, 0);
        }

        if (ascending && transform.position.y > middlePos.y + range)
        {
            ascending = false;
        }
        else if (!ascending && transform.position.y < middlePos.y - range)
        {
            ascending = true;
        }
    }
}
