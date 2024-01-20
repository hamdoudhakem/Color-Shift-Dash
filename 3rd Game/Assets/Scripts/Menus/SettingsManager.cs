using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer AudMix;
    public Slider volume;
    public Image VibrationsToggle;
    public Sprite[] Toggles;

    public void LoadSettings()
    {
        volume.value = PlayerData.Sound;

        Sound_EventHandler(PlayerData.Sound);

        ChangeToggleImg();
    }

    void ChangeToggleImg()
    {
        if (PlayerData.Vibrations)
        {
            VibrationsToggle.sprite = Toggles[1];
        }
        else
        {
            VibrationsToggle.sprite = Toggles[0];
        }
    }

    public void Sound_EventHandler(float NewVal)
    {
        float sound = (Mathf.Log10(NewVal) * 20) + 20;

        //Debug.Log("Sound Became : " + sound);

        AudMix.SetFloat("Volume" ,sound != -60 ? sound : -80);

        PlayerData.Sound = NewVal;

        SaveSystem.Save();
    }    

    public void Vibrations_EventHandler(Animator anim)
    {
        PlayerData.Vibrations = !PlayerData.Vibrations;

        if (PlayerData.Vibrations)
        {
            anim.SetTrigger("On");
        }
        else
        {
            anim.SetTrigger("Off");
        }                
    }

    private void OnDisable()
    {
        Debug.Log("I Saved on Disable");
        SaveSystem.Save();
    }
}
