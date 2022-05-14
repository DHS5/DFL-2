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


    [Tooltip("Number of the music currently playing")]
    protected int musicNumber;
    public int MusicNumber
    {
        get { return musicNumber; }
        set { musicNumber = value; if (DataManager.InstanceDataManager != null) DataManager.InstanceDataManager.musicNumber = value; }
    }


    protected bool musicOn;
    public bool MusicOn
    {
        get { return musicOn; }
        set 
        {
            if (value == true && !musicOn) audioSource.UnPause();
            else if (value == false && musicOn) audioSource.Pause();

            musicOn = value;
            if (DataManager.InstanceDataManager != null)
            {
                DataManager.InstanceDataManager.musicOn = value;
                DataManager.InstanceDataManager.SavePlayerData();
            }
        }
    }
    protected float musicVolume;
    public float MusicVolume
    {
        get { return musicVolume; }
        set 
        {
            musicVolume = value;
            audioSource.volume = musicVolume;
            if (DataManager.InstanceDataManager != null)
            {
                DataManager.InstanceDataManager.musicVolume = value;
                DataManager.InstanceDataManager.SavePlayerData();
            }
        }
    }

    protected bool soundOn;
    public bool SoundOn
    {
        get { return soundOn; }
        set 
        { 
            soundOn = value;
            if (DataManager.InstanceDataManager != null)
            {
                DataManager.InstanceDataManager.soundOn = value;
                DataManager.InstanceDataManager.SavePlayerData();
            }
        }
    }
    protected float soundVolume;
    public float SoundVolume
    {
        get { return soundVolume; }
        set
        {
            soundVolume = value;
            if (DataManager.InstanceDataManager != null)
            {
                DataManager.InstanceDataManager.soundVolume = value;
                DataManager.InstanceDataManager.SavePlayerData();
            }
        }
    }


    protected bool loopOn;
    public bool LoopOn
    {
        get { return loopOn; }
        set 
        { 
            loopOn = value;
            if (DataManager.InstanceDataManager != null)
            {
                DataManager.InstanceDataManager.loopOn = value;
                audioSource.loop = value;
                DataManager.InstanceDataManager.SavePlayerData();
            }
        }
    }


    protected virtual void Start()
    {
        audioSource = FindObjectOfType<MusicSource>().audioSource;
    }

    private void Update()
    {
        if (musicOn && !audioSource.isPlaying)
        {
            NextMusic();
        }
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


    /// <summary>
    /// Plays the next music in the list
    /// </summary>
    public void NextMusic()
    {
        if (musicNumber == musics.Length - 1) MusicNumber = 0;
        else MusicNumber++;

        PlayFromBeginning(musicNumber);
    }
    /// <summary>
    /// Plays the previous music in the list
    /// </summary>
    public void PreviousMusic()
    {
        if (musicNumber == 0) MusicNumber = musics.Length - 1;
        else MusicNumber--;

        PlayFromBeginning(musicNumber);
    }
}
