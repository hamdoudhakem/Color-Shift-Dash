using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LvsManager : MonoBehaviour
{
    public Transform Sliders;
    public Animator LoadScreen;

    public void LoadLvsData()
    {
        int LvsPerSlider = Sliders.GetChild(0).childCount;

        ShowLvStars(Sliders.GetChild(0).GetChild(0).GetChild(1), 1);

        for (int i = 1; i < PlayerData.CurrentLv; i++)
        {
            Transform CurLv = Sliders.GetChild(i / LvsPerSlider).GetChild(i % LvsPerSlider);

            CurLv.GetChild(1).gameObject.SetActive(false);

            ShowLvStars(CurLv.GetChild(2), i+1);            
        }        
    }

    void ShowLvStars(Transform Stars, int lvNum)
    {      
        for (int i = 0; i < PlayerData.LvXStars[lvNum]; i++)
        {
            Stars.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ChooseLv(TextMeshProUGUI Lv)
    {
        int lv = int.Parse(Lv.text);

        if(lv <= PlayerData.CurrentLv)
        {
            LoadScreen.gameObject.SetActive(true);
            LoadScreen.SetTrigger("Appear");
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(lv);
        }
        
    }


}
