using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bomb : MonoBehaviour {
  [SerializeField]
  private AudioSource _boomAudio;
  [SerializeField]
  private bool _includeLauncher;
  private bool m_active;
  private Unit m_launcher;
  public float atkScale { private set; get; }

  public void OnBoom(Unit launcher, float atkScale, Vector3 initPos) {
    m_active = true;
    m_launcher = launcher;
    this.atkScale = atkScale;
    initPos.z = transform.position.z;
    transform.position = initPos;
    if (_boomAudio != null) {
      _boomAudio.Play();
    }
    CameraController.DoShake(0.2f, 1.0f);
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (!m_active) {
      return;
    }

    if (collision.CompareTag("Destroyable")) {
      Destroy(collision.gameObject);
      return;
    }

    var target = collision.GetComponent<Unit>();
    if (target != null && (_includeLauncher || target != m_launcher) && target.isAlive) {
      target.DoHurt(m_launcher.atk * atkScale);
    }
  }

  private void _OnRecycle() {
    gameObject.SetActive(false);
  }

  private void _OnInactive() {
    m_active = false;
  }
}