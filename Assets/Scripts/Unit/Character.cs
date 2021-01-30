using UnityEngine;

public class Character : Unit {
  private float m_originGravity;

  protected override void Start() {
    base.Start();

    m_originGravity = gravity;

    //Down和Up事件需要特殊处理
    InputManager.RegisterDownEvent(PressKey.JUMP, OnJump);
    InputManager.RegisterDownEvent(PressKey.SWITCH_WEAPON, OnSwitchWeapon);
  }

  protected override void OnDie() {
    base.OnDie();
    //Down和Up事件需要特殊处理
    InputManager.RemoveDownEvent(PressKey.JUMP, OnJump);
    InputManager.RemoveDownEvent(PressKey.SWITCH_WEAPON, OnSwitchWeapon);
  }

  protected override void UpdateAnimState() {
    base.UpdateAnimState();
    animator.SetBool("move", (InputManager.GetButton(PressKey.LEFT) ||
    InputManager.GetButton(PressKey.RIGHT)) && (jumpManager?.isGrounded ?? false));
  }

  protected override void FixedUpdate() {
    if (!isAlive) {
      return;
    }

    //移动
    tempVel = Vector2.zero;
    if (InputManager.GetButton(PressKey.LEFT)) {
      spriteRenderer.flipX = false;
      tempVel.x -= attribute.moveSpeed;
    }
    if (InputManager.GetButton(PressKey.RIGHT)) {
      spriteRenderer.flipX = true;
      tempVel.x += attribute.moveSpeed;
    }
    tempVel.y = vel.y;

    //攻击
    if (InputManager.GetButton(PressKey.ATTACK)) {
      currentWeapon?.OnAttack();
    }

    //应用速度
    vel = tempVel + attribute.extraVel;

    //角色跳起和下落的重力不同，下落的时候重力更大点以改善跳跃手感
    gravity = m_originGravity * (vel.y > 0 ? 1.0f : 1.25f);
  }
}