using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuBehavior : MonoBehaviour
{
    private SlidesNavig Sn;

    private void Start()
    {
        Sn = GetComponent<SlidesNavig>();
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
