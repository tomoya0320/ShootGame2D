using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit {
  private int m_mode = -1;
  [SerializeField]
  private float _switchModeDelta = 10.0f;
  private Timer m_timer = new Timer();

  protected override void FixedUpdate() {
    if (!isAlive) {
      return;
    }
    m_timer.OnUpdate(Time.fixedDeltaTime);
    if (m_timer.ready) {
      m_mode = (m_mode + 1) % 2;
      curWeaponIdx = m_mode;
      m_timer.OnStart(Time.fixedTime, _switchModeDelta);
    }

    //攻击
    currentWeapon?.OnAttack();

    /*switch (m_mode) {
      case 0: {
          simulated = true;

          if (Mathf.Abs(vel.x) < MathUtil.epsilon) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
          }

          //随机跳跃
          if (MathUtil.JudgeProb(0.005f)) {
            OnJump();
          }
          break;
        }
      case 1: {
          simulated = false;

          if (MathUtil.JudgeProb(0.005f)) {
            tempVel.y = Random.Range(-1.0f, 1.0f);
          }

          transform.position += Time.fixedDeltaTime * new Vector3(tempVel.x, tempVel.y);
          break;
        }
    }

    //移动
    tempVel = Vector2.zero;
    tempVel.x += direction * attribute.moveSpeed;
    tempVel.y = vel.y;

    //应用速度
    vel = tempVel + attribute.extraVel;*/
  }
}