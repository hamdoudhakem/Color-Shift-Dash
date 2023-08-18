using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;         
    
    public AudioClip Clip;
    
    [Range(0f , 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float SpacialBlend;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
