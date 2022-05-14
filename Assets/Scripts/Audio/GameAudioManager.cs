using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : AudioManager
{
    protected override void Start()
    {
        base.Start();
        
        if (DataManager.InstanceDataManager != null)
        {
            MusicOn = DataManager.InstanceDataManager.musicOn; musicToggle.isOn = musicOn;
            MusicVolume = DataManager.InstanceDataManager.musicVolume; musicSlider.value = musicVolume;

            SoundOn = DataManager.InstanceDataManager.soundOn; soundToggle.isOn = soundOn;
            SoundVolume = DataManager.InstanceDataManager.soundVolume; soundSlider.value = soundVolume;

            MusicNumber = DataManager.InstanceDataManager.musicNumber;
            LoopOn = DataManager.InstanceDataManager.loopOn; loopToggle.isOn = loopOn;
        }
        else
        {
            MusicOn = musicToggle.isOn;
            MusicVolume = musicSlider.value;

            SoundOn = soundToggle.isOn;
            SoundVolume = soundSlider.value;

            MusicNumber = 0;
            LoopOn = loopToggle.isOn;
            PlayFromBeginning(musicNumber);
        }
    }


    /// <summary>
    /// Puts the volume of all the sound audio sources to the desired volume
    /// </summary>
    public void ActuSoundVolume()
    {
        foreach (AudioSource a in FindObjectsOfType<AudioSource>())
        {
            if (!a.CompareTag("AudioManager"))
            {
                a.volume = soundVolume;
            }
        }
    }

    /// <summary>
    /// Mutes the game sounds
    /// </summary>
    /// <param name="tmp"></param>
    public void MuteSound(bool tmp)
    {
        foreach (AudioSource a in FindObjectsOfType<AudioSource>())
        {
            if (!a.CompareTag("AudioManager"))
            {
                a.mute = tmp;
            }
        }
    }
}
