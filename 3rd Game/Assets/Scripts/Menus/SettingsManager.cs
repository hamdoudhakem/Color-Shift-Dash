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
        float sound = (Mathf.Log10(NewVal) * 20) + 20;

        Debug.Log("Sound Became : " + sound);

        AudMix.SetFloat("Volume" ,sound != -60 ? sound : -80);

        PlayerData.Sound = NewVal;

        SaveSystem.Save();
    }

    public void LoadSettings()
    {     
        volume.value = PlayerData.Sound;

        Sound_EventHandler(PlayerData.Sound);
    }
}
