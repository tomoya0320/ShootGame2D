
using UnityEngine;

public class Enemy : Unit {
  protected override void FixedUpdate() {
    if (!isAlive) {
      return;
    }

    //移动
    tempVel = Vector2.zero;
    if (Mathf.Abs(vel.x) < MathUtil.epsilon) {
      spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    tempVel.x += direction * attribute.moveSpeed;
    tempVel.y = vel.y;

    //随机跳跃
    if (Random.Range(0, 1.0f) < 0.005f) {
      OnJump();
    }

    //攻击
    currentWeapon?.OnAttack();

    //应用速度
    vel = tempVel + attribute.extraVel;
  }
}