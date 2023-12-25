using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HelpMenuBehavior : MonoBehaviour
{
    [Tooltip("The First Menu Videos to prepare them for play")]
    public VideoPlayer[] GameplayVids;
    private SlidesNavig Sn;

    private void Start()
    {
        Sn = GetComponent<SlidesNavig>();

        foreach (VideoPlayer vid in GameplayVids)
        {
            vid.Prepare();
        }
    }

    public void SetUpSlideNavi(Transform TabVids)
    {
        if(Sn.CurSlides != TabVids)
        {
            Sn.ResetSlide();
            Sn.SetUpNavig(null, TabVids, TabVids.parent.GetChild(1));
        }           
    }
}
