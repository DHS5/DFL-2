using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSource : MonoBehaviour
{
    public static MusicSource InstanceMusicSource { get; private set; }

    [SerializeField] private DataManager dataManager;


    public AudioSource audioSource;

    [Header("Music's list")]
    [Tooltip("Music's list")]
    [SerializeField] private PlaylistSO musics;

    [Header("UI elements")]
    [Tooltip("Music's volume slider")]
    [SerializeField] private Slider musicSlider;
    [Tooltip("Music On toggle")]
    [SerializeField] private Toggle musicOnToggle;
    [Tooltip("Loop On toggle")]
    [SerializeField] private Toggle loopOnToggle;


    private int musicNumber;


    // ### Properties ###
    /// <summary>
    /// Index of the music in the musics list
    /// </summary>
    public int MusicNumber
    {
        get { return musicNumber; }
        set 
        { 
            musicNumber = value;
            dataManager.audioData.musicNumber = value;
        }
    }
    /// <summary>
    /// Whether the music is playing
    /// </summary>
    public bool MusicOn
    {
        get { return musicOnToggle.isOn; }
        set
        {
            if (value) audioSource.UnPause();
            else if (!value) audioSource.Pause();

            dataManager.audioData.musicOn = value;
        }
    }
    /// <summary>
    /// Volume of the music
    /// </summary>
    public float MusicVolume
    {
        get { return musicSlider.value; }
        set
        {
            audioSource.volume = MusicVolume;
            dataManager.audioData.musicVolume = value;
        }
    }
    /// <summary>
    /// Whether the musics are looping
    /// </summary>
    public bool LoopOn
    {
        get { return loopOnToggle.isOn; }
        set
        {
            audioSource.loop = value;
            dataManager.audioData.loopOn = value;
        }
    }




    private void Awake()
    {
        if (InstanceMusicSource != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceMusicSource = this;
        DontDestroyOnLoad(gameObject);

        audioSource.ignoreListenerPause = true;
    }

    private void Start()
    {
        LoadAudioData(dataManager.audioData);
    }

    private void Update()
    {
        if (MusicOn && !audioSource.isPlaying) // Plays the next music when the previous has ended
        {
            NextMusic();
        }
    }


    // ### Functions ###

    public void LoadAudioData(AudioData data)
    {
        MusicOn = data.musicOn;
        musicOnToggle.isOn = data.musicOn;

        MusicVolume = data.musicVolume;
        musicSlider.value = data.musicVolume;

        MusicNumber = data.musicNumber;

        LoopOn = data.loopOn;
        loopOnToggle.isOn = data.loopOn;

        if (MusicOn)
            PlayFromBeginning(musicNumber);
    }


    /// <summary>
    /// Plays the chosen clip from the beginning
    /// </summary>
    /// <param name="clipNumber">Number of the music clip</param>
    protected void PlayFromBeginning(int clipNumber)
    {
        if (clipNumber >= musics.musics.Length)
        {
            MusicNumber = 0;
            clipNumber = 0;
        }
        audioSource.clip = musics.musics[clipNumber];
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
        audioSource.clip = musics.musics[clipNumber];
        audioSource.time = startTime;
        audioSource.Play();
    }

    /// <summary>
    /// Plays the next music in the list
    /// </summary>
    public void NextMusic()
    {
        if (musicNumber == musics.musics.Length - 1) MusicNumber = 0;
        else MusicNumber++;

        PlayFromBeginning(musicNumber);
    }
    /// <summary>
    /// Plays the previous music in the list
    /// </summary>
    public void PreviousMusic()
    {
        if (musicNumber == 0) MusicNumber = musics.musics.Length - 1;
        else MusicNumber--;

        PlayFromBeginning(musicNumber);
    }
}
