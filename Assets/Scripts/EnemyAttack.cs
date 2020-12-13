using UnityEngine;

public class EnemyAttack : MonoBehaviour {
  [SerializeField]
  private Unit _enemy;
  [SerializeField]
  private float _attackDelta = 1.0f;
  private Timer m_timer;

  private void Awake() {
    m_timer = new Timer();
  }

  private void FixedUpdate() {
    m_timer.OnUpdate(Time.fixedDeltaTime);
  }

  private void OnTriggerStay2D(Collider2D collision) {
    if (!m_timer.ready || !_enemy.isAlive) {
      return;
    }

    var unit = collision.GetComponent<Unit>();
    if(unit !=null && unit.isAlive && unit is Character ) {
      m_timer.OnStart(Time.fixedTime, _attackDelta);
      unit.DoHurt(_enemy.atk);
    }
  }
}