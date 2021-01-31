using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    public CanvasGroup name;
    private float i;//用来记录透明度改变
    private bool open;//用来记录进入还是退出
    private bool hasOpen = false;//用来记录是否打开过

    void Start()
    {
        i = 0.01f;
        open = true;
    }

    void Update()
    {
        name.alpha = i;
        if (open)
            i += Time.deltaTime;
        else i -= Time.deltaTime;
        if (i >= 2)
            open = false;
        if (i <= 0)
        {
            SceneManager.LoadScene("game");
        }
    }
}
