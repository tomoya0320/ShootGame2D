using UnityEngine;

public class EnemyHead : MonoBehaviour {
  [SerializeField]
  private Unit _enemy;
  [SerializeField]
  private float _boundSpeed = 20.0f;
    [SerializeField]
    private bool isBird = false;


    private void OnTriggerEnter2D(Collider2D collision) {
    if (!_enemy.isAlive) {
      return;
    }

    if (collision.name == "foot") {
      var unit = collision.GetComponent<JumpManager>()?.unit;
      if (unit != null && unit.isAlive && unit is Character && unit.vel.y < MathUtil.epsilon) {
        unit.Bound(_boundSpeed * Vector2.up);
        _enemy.DoHurt(unit.atk);
      }
    }
  }
}