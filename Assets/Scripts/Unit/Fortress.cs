using UnityEngine;

public class Fortress : Unit {
  [SerializeField]
  private float _switchWeaponDelta;
  private Timer m_timer = new Timer();

  protected override void Start() {
    base.Start();

    curWeaponIdx = -1;
  }

  protected override void FixedUpdate() {
    if (!isAlive) {
      return;
    }
    m_timer.OnUpdate(Time.fixedDeltaTime);
    if (m_timer.ready) {
      OnSwitchWeapon();
      m_timer.OnStart(Time.fixedTime, _switchWeaponDelta);
    }

    currentWeapon?.OnAttack();
  }
}