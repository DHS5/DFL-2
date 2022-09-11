using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzer : MonoBehaviour
{
    [SerializeField] private ParticleSystem rightConfetti;
    [SerializeField] private ParticleSystem leftConfetti;


    private void OnCollisionEnter(Collision collision)
    {
        rightConfetti.Play();
        leftConfetti.Play();
    }
}
