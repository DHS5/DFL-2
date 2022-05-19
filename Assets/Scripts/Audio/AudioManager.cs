using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AudioManager : MonoBehaviour
{
    protected AudioSource audioSource;

    [SerializeField] protected AudioClip[] musics;


    [SerializeField] protected Toggle musicToggle;
    [SerializeField] protected Slider musicSlider;

    [SerializeField] protected Toggle soundToggle;
    [SerializeField] protected Slider soundSlider;

    [SerializeField] protected Toggle loopToggle;


    


    protected virtual void Start()
    {
        audioSource = FindObjectOfType<MusicSource>().audioSource;
    }



    /// <summary>
    /// Plays the chosen clip from the beginning
    /// </summary>
    /// <param name="clipNumber">Number of the music clip</param>
    protected void PlayFromBeginning(int clipNumber)
    {
        audioSource.clip = musics[clipNumber];
        audioSource.time = 0f;
        audioSource.Play();
    }
    /// <summary>
    /// Plays the chosen clip from the chosen start time
    /// </summary>
    /// <param name="clipNumber">Number of the music clip</param>
    /// <param name="startTime">Start time of the music clip</param>
    protected void PlayFromTime(int clipNumber, float startTime)
    {
        audioSource.clip = musics[clipNumber];
        audioSource.time = startTime;
        audioSource.Play();
    }

}
