using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAudioManager : MonoBehaviour
{
    [Tooltip("Main Manager")]
    private MainManager main;

    [SerializeField] private AudioSource ambianceAudioSource;
    
    private AudioSource[] audioSources;
    private AudioSource[] soundSources;

    [Header("Sounds bank")]
    [SerializeField] private AudioClip[] playerEntryClips;
    [SerializeField] private AudioClip[] touchdownCelebrationClips;
    [SerializeField] private AudioClip[] bigplayReactionClips;
    [SerializeField] private AudioClip[] surpriseClips;
    [SerializeField] private AudioClip[] bouhClips;
    [SerializeField] private AudioClip[] winClips;
    [SerializeField] private AudioClip[] zombieAmbianceClips;


    [Header("Audio mixer group")]
    [SerializeField] private AudioMixer audioMixer;


    private AudioSource[] playerAudioSources;


    private bool crowdSound = true;

    // ### Properties ###

    public float SoundVolume
    {
        get { return Mathf.Log10(main.DataManager.audioData.soundVolume) * 20; }
        set
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        }
    }
    public bool SoundOn
    {
        get { return main.DataManager.audioData.soundOn; }
        set { audioMixer.SetFloat("Volume", value ? SoundVolume : -80); }
    }


    private void Awake()
    {
        main = GetComponent<MainManager>();
    }



    // ### Functions ###

    public void Pause(bool state)
    {
        //if (state) GetSoundSources();

        PauseSound(state || !SoundOn);
    }


    public void GetSoundSources()
    {
        audioSources = FindObjectsOfType<AudioSource>();

        List<AudioSource> list = new List<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            if (!source.CompareTag("MusicSource"))
                list.Add(source);
        }
        soundSources = list.ToArray();
    }

    public void GenerateAudio()
    {
        GetSoundSources();

        // here case by case
        if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE || (main.GameManager.gameData.gameMode == GameMode.DRILL && main.GameManager.gameData.gameDrill != GameDrill.PARKOUR))
        {
            crowdSound = false;
            main.FieldManager.stadium.MuteBleachersSound();
            if (main.GameManager.gameData.gameMode == GameMode.ZOMBIE)
            {
                ambianceAudioSource.clip = zombieAmbianceClips[Random.Range(0, zombieAmbianceClips.Length)];
                ambianceAudioSource.Play();
            }
        }
        else
        {
            crowdSound = main.FieldManager.stadium.crowd;
            if (crowdSound) PlayerEntry();
        }

        AudioData data = main.DataManager.audioData;
        if (data.soundOn)
        {
            MuteSound(false);
            ActuSoundVolume();
        }
        else
        {
            MuteSound(true);
        }
    }



    /// <summary>
    /// Puts the volume of all the sound audio sources to the desired volume
    /// </summary>
    private void ActuSoundVolume()
    {
        SoundVolume = main.DataManager.audioData.soundVolume;
    }

    /// <summary>
    /// Mutes the game sounds
    /// </summary>
    /// <param name="tmp"></param>
    public void MuteSound(bool tmp)
    {
        foreach (AudioSource a in soundSources)
        {
            a.mute = tmp;
        }
    }
    public void PauseSound(bool state)
    {
        AudioListener.pause = state;
    }

    public void Lose()
    {
        if (crowdSound)
        {
            main.FieldManager.stadium.MuteBleachersSound();
            OuuhAudio();
            Invoke(nameof(BouhAudio), playerAudioSources[0].clip.length - 1.5f);
        }
    }

    private void BouhAudio()
    {
        PlayOneShot(bouhClips[Random.Range(0, bouhClips.Length)]);
    }

    private void OuuhAudio()
    {
        PlayClip(surpriseClips[Random.Range(0, surpriseClips.Length)]);
    }

    public void PlayerEntry()
    {
        playerAudioSources = main.PlayerManager.player.crowdAudioSources;
        if (crowdSound)
        {
            PlayClip(playerEntryClips[Random.Range(0, playerEntryClips.Length)]);
        }
    }
    public void TouchdownCelebration()
    {
        if (crowdSound)
        {
            PlayOneShot(touchdownCelebrationClips[Random.Range(0, touchdownCelebrationClips.Length)]);
        }
    }
    public void BigplayReaction()
    {
        if (crowdSound)
        {
            PlayOneShot(bigplayReactionClips[Random.Range(0, bigplayReactionClips.Length)]);
        }
    }

    public void Win()
    {
        PlayClip(winClips[Random.Range(0, winClips.Length)]);
    }

    private void PlayClip(AudioClip clip)
    {
        foreach (AudioSource a in playerAudioSources)
        {
            a.clip = clip;
            a.Play();
        }
    }
    private void PlayOneShot(AudioClip clip)
    {
        foreach (AudioSource a in playerAudioSources)
        {
            a.PlayOneShot(clip);
        }
    }
}
