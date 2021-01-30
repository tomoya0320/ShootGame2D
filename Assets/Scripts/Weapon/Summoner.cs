using UnityEngine;

public class Summoner : Weapon {
  [SerializeField]
  private int _maxCount;
  [SerializeField]
  private GameObject _summoned;
  [SerializeField]
  private Transform _bornPoint;
  [SerializeField]
  private Transform _folder;
  private int m_currentCount;

  public override void OnAttack() {
    if (timer.ready && !owner.isSwitchingWeapon && m_currentCount < _maxCount) {
      m_currentCount++;
      var tempGo = Instantiate(_summoned, _bornPoint.position, Quaternion.identity, _folder);
      tempGo.GetComponent<SpriteRenderer>().flipX = !owner.flipX;
      timer.OnStart(Time.fixedTime, _attackDelta);
    }
  }

  public override void OnSwitchIn() {
    timer.OnStart(Time.fixedTime, _attackDelta);
  }

  public override void OnSwitchOut() {
    m_currentCount = 0;
  }
}