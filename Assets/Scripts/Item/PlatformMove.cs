using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMove : MonoBehaviour
{
    public float xDis;//水平移动距离
    public float yDis;//竖直移动距离
    public float time;//完成的时间

    private Vector3 myPos;
    private Vector3 from;
    private Vector3 to;

    void Start()
    {
        myPos = this.transform.position;
        from = new Vector3(myPos.x, myPos.y, myPos.z);
        to = new Vector3(myPos.x + xDis, myPos.y + yDis, myPos.z);

        Move(from, to);
    }

    private void Move(Vector3 from, Vector3 to)
    {
        transform.DOMove(to, time).SetEase(Ease.Linear).OnComplete(() => Move(to, from));
    }
}
