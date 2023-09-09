using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public List<ObsTypes> ConcernedTypes;
    [Tooltip("Doors Layer")]
    public LayerMask ConcernedLayers;

    [Tooltip("The Difference between every")]
    public float CallDelay;

    [Tooltip("The Distance from which the player will start hearing the sounds")]
    public float SoundDis;
    public Vector3 BoxSize;
    public Vector3 Offset;

    private RaycastHit[] hits;
    private bool StartVolumeChange;
    private float DefaultDoorVol;

    /// <summary>
    /// I Need To Keep Comparing The lastTrans detected with the New One and when the new One Becames Closer
    /// To the Player then i Will use it to choose the volume instead of the last one
    /// </summary>
    private Transform LastTrans;

    void Start()
    {
        StartVolumeChange = false;

        DefaultDoorVol = AudioManager.AudMan.GetVolume("Doors");

        InvokeRepeating("VolumeChecker", 0, CallDelay);
    }

    void VolumeChecker()
    {
        hits = Physics.BoxCastAll(transform.position + Offset, BoxSize, Vector3.forward, new Quaternion(), SoundDis, ConcernedLayers);

        if (hits.Length > 0)
        {
            RaycastHit hit = Min(hits, hit => Mathf.Abs(transform.position.z - hit.transform.position.z));

            Transform par = hit.transform;

            if(par != LastTrans)
            {
                do
                {
                    if (par.TryGetComponent(out IObsTypes type))
                    {
                        if (ConcernedTypes.Contains(type.obsType))
                        {
                            StartVolumeChange = true;
                        }

                        break;
                    }

                    par = par.parent;

                } while (par != null);

                if (StartVolumeChange)
                {
                    LastTrans = par;

                    StopAllCoroutines();
                    StartCoroutine(UpdateVolume(hit.transform));

                    StartVolumeChange = false;
                }
            }
            
        }
    }

    IEnumerator UpdateVolume(Transform trans)
    {
        float NewVol , Dif;

        do
        {
            yield return new WaitForSeconds(.025f);

            Dif = Mathf.Abs(trans.position.z - transform.position.z);

            float LosePercentage = Dif / SoundDis;

            NewVol = DefaultDoorVol * (1 - LosePercentage);

            AudioManager.AudMan.SetVolume("Doors", NewVol);

        } while (Dif > SoundDis);

        trans.parent.gameObject.SetActive(false);

        AudioManager.AudMan.Stop("Doors");

        AudioManager.AudMan.SetVolume("Doors", DefaultDoorVol);
    }


    public T Min<T>(T[] hits, Func<T , float> selector)
    {
        float Min = selector(hits[0]) , NewVal;
        int index = 0;

        for (int i = 1; i < hits.Length; i++)
        {
            NewVal = selector(hits[i]);

            if (NewVal < Min)
            {
                Min = NewVal;
                index = i;
            }
        }

        return hits[index];
    }
}
