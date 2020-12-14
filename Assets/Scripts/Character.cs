using UnityEngine;

public class Character : Unit {
  protected override void Start() {
    base.Start();
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
             InputManager.GetButton(PressKey.RIGHT)) && jumpManager.isGrounded);
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
  }
}