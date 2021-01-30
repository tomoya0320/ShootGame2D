using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy_Bird : Unit
{
    public float speed = 3;
    public Vector3[] begOffset;//射线偏移
    public float[] raycastLen;//射线长度

    public bool flyDir = true;//随机上下
    private float flyTime = 0;//记录飞行时间

    protected override void FixedUpdate()
    {
        if (!isAlive)
            return;

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
        //随机攻击
        if (MathUtil.JudgeProb(0.01f))
            currentWeapon?.OnAttack();
        //应用速度
        vel = tempVel + attribute.extraVel;
    }
}
