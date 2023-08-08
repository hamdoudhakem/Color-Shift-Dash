using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer AudMix;
    public Slider volume; 

    public void Sound_EventHandler(float NewVal)
    {
        AudMix.SetFloat("Volume" ,NewVal);

        PlayerData.Sound = NewVal;

        SaveSystem.Save();
    }

    public void LoadSettings()
    {
        AudMix.SetFloat("Volume", PlayerData.Sound);

        volume.value = PlayerData.Sound;
    }
}
