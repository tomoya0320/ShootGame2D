using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bomb : MonoBehaviour {
  [SerializeField]
  private bool _includeLauncher;
  [SerializeField]
  private float _boomRange = 1.0f;
  private bool m_active;
  private Unit m_launcher;
  public float atkScale { private set; get; }
  private HashSet<Unit> m_damagedUnits = new HashSet<Unit>();

  private void Awake() {
    transform.localScale *= _boomRange;
  }

  public void OnBoom(Unit launcher, float atkScale,Vector3 initPos) {
    m_active = true;
    m_launcher = launcher;
    this.atkScale = atkScale;
    initPos.z = transform.position.z;
    transform.position = initPos;
    CameraController.DoShake(0.2f, 1.0f);
  }

  private void OnTriggerStay2D(Collider2D collision) {
    if (!m_active) {
      return;
    }

    var target = collision.GetComponent<Unit>();
    if (target != null && (_includeLauncher || target != m_launcher) && target.isAlive && !m_damagedUnits.Contains(target)) {
      target.DoHurt(m_launcher.atk * atkScale);
      m_damagedUnits.Add(target);
    }
  }

  private void _OnRecycle() {
    m_damagedUnits.Clear();
    gameObject.SetActive(false);
  }

  private void _OnInactive() {
    m_active = false;
  }
}