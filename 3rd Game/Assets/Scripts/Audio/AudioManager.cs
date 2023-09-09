using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudMan; 

    public AudioMixerGroup mixer;
    public List<Sound> sounds;

    private bool Paused;

    void Awake()
    {
        Paused = false;

        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();            

            AssignSToAS(sounds[i].source, sounds[i]);
        }

        AudMan = this;
    }

    private void AssignSToAS(AudioSource AS, Sound s)
    {
        AS.clip = s.Clip;
        AS.volume = s.volume;
        AS.pitch = s.pitch;
        AS.loop = s.loop;
        AS.spatialBlend = s.SpacialBlend;

        AS.outputAudioMixerGroup = mixer;
    }   

    public void Play(string name, bool Override = false)
    {
        Sound s = sounds.Find(sound => sound.name.Contains(name));

        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + " doesn't Exist!");
            return;
        }

        if (!Paused)
        {
            if (Override || !s.source.isPlaying)
            {
                //Debug.Log("I'm Playing the " + name + " Sound");
                s.source.Play();
            }
        }

    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name.Contains(name));

        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + " doesn't Exist!");
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }

    }

    public void StopAll()
    {
        foreach (var s in sounds)
        {
            if (s.source.isPlaying)
            {
                s.source.Stop();
            }

        }
    }

    public void PauseAll()
    {
        Paused = true;

        foreach (var s in sounds)
        {        
            if (s.source.isPlaying)
            {
                s.source.Pause();
            }

        }
    }

    public void UnPauseAll()
    {
        Paused = false;

        foreach (var s in sounds)
        {
            s.source.UnPause();
        }
    }

    public void SetVolume(string name,float NewVol)
    {
        Sound s = sounds.Find(sound => sound.name.Contains(name));

        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + " doesn't Exist!");
            return;
        }

        s.source.volume = NewVol;        
    }

    public float GetVolume(string name)
    {
        Sound s = sounds.Find(sound => sound.name.Contains(name));

        if (s == null)
        {
            Debug.LogWarning("Sound :" + name + " doesn't Exist!");
            return -1;
        }

        return s.source.volume;
    }
  
}
