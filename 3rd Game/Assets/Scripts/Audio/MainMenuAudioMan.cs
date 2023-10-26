using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioMan : MonoBehaviour
{
    public static MainMenuAudioMan MaAud;

    public AudioSource Click;
    public AudioSource SettingClick;
    public AudioSource SelectOrEquip;
    public AudioSource Buy;    
    public AudioSource SwitchLvTab;    

    void Awake()
    {
        MaAud = this;
    }

   
}
