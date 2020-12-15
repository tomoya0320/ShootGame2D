using UnityEngine;

public class Enemy : Unit {
  protected override void FixedUpdate() {
    if (!isAlive) {
      return;
    }

    //移动
    tempVel = Vector2.zero;
    if (MathUtil.JudgeProb(0.001f) || Mathf.Abs(vel.x) < MathUtil.epsilon) {
      spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    tempVel.x += direction * attribute.moveSpeed;
    tempVel.y = vel.y;

    //随机跳跃
    if (MathUtil.JudgeProb(0.005f)) {
      OnJump();
    }

    //攻击
    currentWeapon?.OnAttack();

    //应用速度
    vel = tempVel + attribute.extraVel;
  }
}