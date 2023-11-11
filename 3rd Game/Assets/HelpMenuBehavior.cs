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

    public void UpdateSlideNavi()
    {
       // Sn.SetUpNavig(null, )
    }
}
