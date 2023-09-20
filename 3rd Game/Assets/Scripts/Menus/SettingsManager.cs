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
        AudMix.SetFloat("Volume" ,(Mathf.Log10(NewVal) * 20) + 20);

        PlayerData.Sound = NewVal;

        SaveSystem.Save();
    }

    public void LoadSettings()
    {
        AudMix.SetFloat("Volume", (Mathf.Log10(PlayerData.Sound) * 20) + 20);

        volume.value = PlayerData.Sound;
    }
}
