using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    private int m_mode = -1;
    [SerializeField]
    private float _switchModeDelta = 10.0f;
    private Timer m_timer = new Timer();

    //地面模式
    public Vector3 frontbegOffset;//前方射线发出点的偏移
    public Vector3 downbegOffset;//下方发出点的偏移
    public float frontRaycastLen;//前方向检测射线的长度
    public float downRaycastLen;//前下方向检测射线的长度

    //飞行模式
    public float speed = 3;
    public Vector3[] begOffset;//射线偏移
    public float[] raycastLen;//射线长度

    public bool flyDir = true;//随机上下
    private float flyTime = 0;//记录飞行时间

    protected override void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }
        m_timer.OnUpdate(Time.fixedDeltaTime);
        if (m_timer.ready)
        {
            m_mode = (m_mode + 1) % 2;
            curWeaponIdx = m_mode;
            m_timer.OnStart(Time.fixedTime, _switchModeDelta);
        }

        //攻击
        currentWeapon?.OnAttack();

        switch (m_mode)
        {
            case 0:
                {
                    //调整重力
                    gravity = 1.5f;
                    //移动
                    Vector3 front = new Vector3(-1, 0, 0);
                    Vector3 down = new Vector3(0, -1, 0);
                    if (spriteRenderer.flipX == true)
                        front = new Vector3(1, 0, 0);

                    Vector3 frontBeg = transform.position + frontbegOffset;
                    UnityEngine.Debug.DrawLine(frontBeg, frontBeg + front * frontRaycastLen, Color.red);
                    if (Physics2D.Raycast(frontBeg, front, frontRaycastLen, LayerMask.GetMask("Default")))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(frontBeg, front, frontRaycastLen, LayerMask.GetMask("Default"));
                        if (hit.collider != null && hit.collider.tag != "IgnoreMissile")
                            spriteRenderer.flipX = !spriteRenderer.flipX;//悬崖
                    }

                    Vector3 downBeg = transform.position + downbegOffset;
                    UnityEngine.Debug.DrawLine(downBeg, downBeg + (front + down).normalized * downRaycastLen, Color.red);
                    if (Physics2D.Raycast(downBeg, (front + down).normalized, downRaycastLen, LayerMask.GetMask("Default")))
                    {
                        //前面有墙
                    }
                    tempVel = new Vector2(front.x * speed, vel.y);

                    //随机跳跃
                    if (MathUtil.JudgeProb(0.005f))
                        OnJump();
                    //随机转向
                    if (MathUtil.JudgeProb(0.005f))
                        spriteRenderer.flipX = !spriteRenderer.flipX;
                    break;
                }
            case 1:
                {
                    //调整重力
                    gravity = 0.1f;

                    //移动
                    Vector3 front = new Vector3(-1, 0, 0);
                    Vector3 down = new Vector3(0, -1, 0);
                    if (spriteRenderer.flipX == true)
                        front = new Vector3(1, 0, 0);

                    //射线检测
                    int num = begOffset.Length;
                    Vector3[] beg = new Vector3[num];
                    for (int i = 0; i < num; i++)
                        beg[i] = transform.position + begOffset[i];
                    UnityEngine.Debug.DrawLine(beg[0], beg[0] + (front).normalized * raycastLen[0], Color.red);
                    UnityEngine.Debug.DrawLine(beg[1], beg[1] + (front).normalized * raycastLen[1], Color.red);
                    UnityEngine.Debug.DrawLine(beg[2], beg[2] + (front + down).normalized * raycastLen[2], Color.red);
                    UnityEngine.Debug.DrawLine(beg[3], beg[3] + (front - down).normalized * raycastLen[3], Color.red);

                    if (Physics2D.Raycast(beg[0], (front + down).normalized, raycastLen[0], LayerMask.GetMask("Default")) ||
                        Physics2D.Raycast(beg[1], (front - down).normalized, raycastLen[1], LayerMask.GetMask("Default")))
                    {
                        //前面有墙
                    }

                    if (Physics2D.Raycast(beg[2], (front + down).normalized, raycastLen[2], LayerMask.GetMask("Default")))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(beg[2], (front + down).normalized, raycastLen[2], LayerMask.GetMask("Default"));
                        if (hit.collider != null && hit.collider.tag != "IgnoreMissile")
                            flyDir = true;//上面有墙
                    }
                    if (Physics2D.Raycast(beg[3], (front - down).normalized, raycastLen[3], LayerMask.GetMask("Default")))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(beg[3], (front - down).normalized, raycastLen[3], LayerMask.GetMask("Default"));
                        if (hit.collider != null && hit.collider.tag != "IgnoreMissile")
                            flyDir = false;//下面有墙
                    }
                    if (Mathf.Abs(vel.y) > 5f)
                        flyDir = !(flyDir);//防止超速

                    if (flyDir)
                        flyTime = -0.1f;
                    else
                        flyTime = 0.1f;
                    tempVel = new Vector2(front.x * speed, vel.y + flyTime);

                    //随机上下，下的概率更大
                    if (MathUtil.JudgeProb(0.01f))
                        flyDir = false;
                    if (MathUtil.JudgeProb(0.02f))
                        flyDir = true;
                    //随机左右
                    if (MathUtil.JudgeProb(0.01f))
                        spriteRenderer.flipX = !spriteRenderer.flipX;
                    break;
                }
        }
        //应用速度
        vel = tempVel + attribute.extraVel;
    }
}