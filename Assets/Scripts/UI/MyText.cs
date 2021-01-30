using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引用UI的命名空间
using DG.Tweening;
public class MyText : MonoBehaviour
{

    private Text text; //文本

    // Use this for initialization
    void Start()
    {
        text = this.GetComponent<Text>(); // 直接获取游戏物体
        text.DOText("移动：A、D，跳跃：空格键，切换武器：LeftShift，射击：J，退出：ESC", 4);//richtextEnabled 第三个参数是一个富文本
    }

    // Update is called once per frame
    void Update()
    {

    }
}
