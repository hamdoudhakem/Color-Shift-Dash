using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetStars : MonoBehaviour
{
    void Start()
    {
        int CurLv = SceneManager.GetActiveScene().buildIndex;

        for(int i = 1; i < PlayerData.LvXStars[CurLv]; i++)
        {
            transform.GetChild(PlayerData.CollectedStarsIndex[CurLv - 1][i - 1]).gameObject.SetActive(false);
        }
    }

}
