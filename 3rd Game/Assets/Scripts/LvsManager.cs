using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LvsManager : MonoBehaviour
{      
    public void ChooseLv(TextMeshProUGUI Lv)
    {
        int lv = int.Parse(Lv.text);

        //If the Lv is Availble then
        AsyncOperation asyncOp =  SceneManager.LoadSceneAsync(lv);
    }
}
