using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy_Guard : Unit
{
    public float speed = 3;
    public Vector3 frontbegOffset;//前方射线发出点的偏移
    public Vector3 downbegOffset;//下方发出点的偏移
    public float frontRaycastLen;//前方向检测射线的长度
    public float downRaycastLen;//前下方向检测射线的长度

    protected override void FixedUpdate()
    {
        if (!isAlive)
            return;

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
            RaycastHit2D hit = Physics2D.Raycast(downBeg, (front + down).normalized, downRaycastLen, LayerMask.GetMask("Default"));
            if (hit.collider != null && hit.collider.tag != "IgnoreMissile")
                spriteRenderer.flipX = !spriteRenderer.flipX;//有墙
        }
        tempVel = new Vector2(front.x * speed, vel.y);

        //随机转向
        if (MathUtil.JudgeProb(0.002f))
            spriteRenderer.flipX = !spriteRenderer.flipX;
        //随机攻击
        if (MathUtil.JudgeProb(0.02f))
            currentWeapon?.OnAttack();
        //应用速度
        vel = tempVel + attribute.extraVel;
    }
}
