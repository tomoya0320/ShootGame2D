using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public bool victory;
    public GameObject end_1;
    public GameObject end_2;
    public Unit boss2_hp;

    public GameObject[] otherThings;
    public CameraController cc;

    private float i;//用来记录透明度改变
    private int num;//用来记录第一张还是第二张
    private bool open;//用来记录进入还是退出

    void Start()
    {
        victory = false;
        open = true;
        num = 1;
        i = 0.01f;
    }

    void Update()
    {
        if (!boss2_hp.IsAlive())
        {
            victory = true;
            //关掉：Game、两个按钮、cc
            cc.enabled = false;
            for (int i = 0; i < otherThings.Length; i++)
                otherThings[i].SetActive(false);
        }

        if (victory)
        {
            if (num == 1)
            {
                end_1.SetActive(true);
                end_1.GetComponent<CanvasGroup>().alpha = i;
                if (open)
                    i += Time.deltaTime;
                else i -= Time.deltaTime;
                if (i >= 2)
                    open = false;
                if (i <= 0)
                {
                    num = 2;
                    open = true;
                    end_1.SetActive(false);
                }
            }
            if(num == 2)
            {
                end_2.SetActive(true);
                end_2.GetComponent<CanvasGroup>().alpha = i;
                if (open)
                    i += Time.deltaTime;
                else i -= Time.deltaTime;
                if (i >= 4)
                    open = false;
                if (i <= 0)
                //重新加载场景
                {
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
    }
}
