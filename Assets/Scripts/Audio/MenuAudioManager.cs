using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : AudioManager
{
    protected override void Start()
    {
        base.Start();

        if (DataManager.InstanceDataManager != null && DataManager.InstanceDataManager.soundVolume != 0)
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
}
