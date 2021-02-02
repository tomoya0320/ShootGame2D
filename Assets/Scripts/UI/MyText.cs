using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引用UI的命名空间
using DG.Tweening;
public class MyText : MonoBehaviour
{
    public bool openText;//是否开启对话
    public Text text;//文本
    public GameObject frame;//对话框

    private bool preOpenText;//之前对话开启的状态
    private int textNum = 1;//对话序列号
    private int preNum;//之前的序列号
    private string[] dialogue;//对话
    private float pointTime = 0f;//点击时间

    void Start()
    {
        preNum = 0;
        preOpenText = openText;
        dialogue = new string[50];
        dialogue[0] = "通讯器：代号233，代号233，收到请回答。（点击对话框继续）";
        dialogue[1] = "主角：答。";
        dialogue[2] = "通讯器：……";
        dialogue[3] = "通讯器：记住你的机甲行动指令，A是左移，D是右移，W是向上举枪，J是射击，Space是跳跃。";
        dialogue[4] = "通讯器：当你陷入僵局时可以尝试环顾四周，濒临绝境按下重生按钮则可以重启机甲。";
        dialogue[5] = "通讯器：我们遗失的能源核心应该就在这附近的一座机关城中，请仔细搜寻。";
        dialogue[6] = "通讯器：左Shift键可以切换你的武器，虽然现在你可能只有1种。";
        dialogue[7] = "通讯器：你可以在这基地中获取新武器，也可以找我们申请火力支援，我们会尽快回答。";
        dialogue[8] = "主角：申请火力支援！";
        dialogue[9] = "通讯器：答。";
        dialogue[10] = "主角：……";

        dialogue[11] = "主角：前面似乎被堵住了，需要新的武器才能爆破。";
        dialogue[12] = "主角：请求火力支援！！";
        dialogue[13] = "通讯器：（只有电流声）";
        dialogue[14] = "主角：通讯接口真不靠谱，这些人都写的什么BUG……";
        dialogue[15] = "主角：西北方向还没探索，去看看有没有什么弹药支援吧。";

        dialogue[16] = "自爆兵：冲啊，守护我们最后的能源！";
        dialogue[17] = "主角：这什么情况啊，代号233请求通讯！";
        dialogue[18] = "通讯器：（传来了嘈杂的枪火声）";
        dialogue[19] = "主角：只能先带走能源了，上吧！";

        dialogue[20] = "主角：那是我们三千万年前遗失的能源核心，现在我们只是把它寻回了而已。";
        dialogue[21] = "主角：这是我们最后能找到的能源了，我们的基地也已经濒临枯竭。";
        dialogue[22] = "Boss：或许当真如此，但是这三千万年间，这里已经孕育了万千生灵。";
        dialogue[23] = "Boss：失去能源核心，这里的一切都将灰飞烟灭，我们别无选择。";
        dialogue[24] = "Boss：纵使神形俱灭，也不能遗失我们的能源！";
        dialogue[25] = "主角：那只能决斗了！";
        dialogue[26] = "Boss：决斗吧！轮到我的回合！";

        dialogue[27] = "主角：当我回到我们的基地时，建筑却在此刻轰然倒塌……";
        dialogue[28] = "新的主角：那是我们三千万年前遗失的能源核心，现在我们只是把它寻回了而已。";
        dialogue[29] = "新的主角：这是我们最后能找到的能源了，我们的基地也已经濒临枯竭。";
        dialogue[30] = "主角：或许当真如此，但是这三千万年间，这里已经孕育了万千生灵。";
        dialogue[31] = "主角：失去能源核心，这里的一切都将灰飞烟灭，我们别无选择。";
        dialogue[32] = "主角：纵使神形俱灭，也不能遗失我们的能源！";
        dialogue[33] = "新的主角：那只能决斗了！";
        dialogue[34] = "主角（Boss）：决斗吧！轮到我的回合！";
    }

    void Update()
    {
        if (preOpenText != openText)
        {
            if (openText)
            {
                frame.SetActive(true);
                textNum += 1;
            }
            else
            {
                frame.SetActive(false);
            }
            preOpenText = openText;
        }
        if (openText)
        {
            pointTime += Time.deltaTime;
            if (textNum != preNum)
            {
                text.text = " ";
                text.DOText(dialogue[preNum], 1.8f).SetEase(Ease.Linear);
                preNum = textNum;
            }
        }
    }

    public void nextText()
    {
        if (pointTime > 2)
        {
            pointTime = 0;
            if (textNum == 11 || textNum == 16 || textNum == 20 || textNum == 27)
            {
                openText = false;
            }
            else
                textNum += 1;
        }
    }
}
