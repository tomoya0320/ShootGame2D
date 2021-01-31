using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    public GameObject picture;
    public GameObject[] otherThings;
    public CameraController cc;
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
        picture.GetComponent<CanvasGroup>().alpha = i;
        if (open)
            i += Time.deltaTime;
        else i -= Time.deltaTime;
        if (i >= 2)
            open = false;
        if (i <= 0)
        {
            if (!hasOpen)
            {
                for (int i = 0; i < otherThings.Length; i++)
                    otherThings[i].SetActive(true);
                cc.enabled = true;
                picture.SetActive(false);
                hasOpen = true;
            }
        }
    }
}
