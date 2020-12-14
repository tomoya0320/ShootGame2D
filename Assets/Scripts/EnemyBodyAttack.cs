using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyAttack : Weapon {
  [SerializeField]
  private int _maxHitNum = 1;
  private List<Unit> m_targets = new List<Unit>();

  public override void OnAttack() {
    if (timer.ready) { 
      timer.OnStart(Time.fixedTime, _attackDelta);

      for (int i = 0; i < _maxHitNum && i < m_targets.Count; i++) { 
        if (m_targets[i].isAlive) {
          m_targets[i].DoHurt(owner.atk * _atkScale);
        }
      }   
    }
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    var unit = collision.GetComponent<Unit>();
    if (unit != null && unit != owner && unit is Character) {
      if (!m_targets.Contains(unit)) {
        if (m_targets.Count == 0) { 
          timer.OnReset();
        }
        m_targets.Add(unit);
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    var unit = collision.GetComponent<Unit>();
    if (unit != null && unit != owner && unit is Character) {
      if (m_targets.Contains(unit)) {
        m_targets.Remove(unit);
      }
    }
  }
}