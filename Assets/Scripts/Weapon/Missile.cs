using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Missile : MonoBehaviour {
  [SerializeField]
  private AudioSource _boomAudio;
  [SerializeField]
  private bool _penetrable;
  [SerializeField]
  private float _moveSpeed;
  [SerializeField]
  private float _maxLifeTime;
  private float m_startTime;
  private Unit m_launcher;
  public float atkScale { private set; get; }
  private bool m_active;
  private Action<Missile> m_onMissileBoom;
  private Animator m_animator;
  private Rigidbody2D m_rigidbody2D;
  private SpriteRenderer m_spriteRenderer;
  public Vector2 vel {
    get {
      return m_rigidbody2D.velocity;
    }
    private set {
      m_rigidbody2D.velocity = value;
    }
  }

  private void Awake() {
    m_animator = GetComponent<Animator>();
    m_rigidbody2D = GetComponent<Rigidbody2D>();
    m_spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void OnLaunch(Unit launcher, float atkScale, float angle, Vector3 initPos, Action<Missile> onMissileBoom = null) {
    this.atkScale = atkScale;
    m_active = true;
    m_launcher = launcher;
    m_startTime = Time.fixedTime;
    m_onMissileBoom = onMissileBoom;
    //位置
    initPos.z = transform.position.z;
    transform.position = initPos;
    //角度
    var eulerAngles = transform.eulerAngles;
    eulerAngles.z = angle * launcher.direction;
    transform.eulerAngles = eulerAngles;
    //移动方向
    var direction = MathUtil.RotateVector2(Vector2.right, angle);
    direction.x *= launcher.direction;
    vel = direction * _moveSpeed;
    //图片水平翻转
    m_spriteRenderer.flipX = launcher.flipX;
  }

  private void FixedUpdate() {
    if (!m_active) {
      return;
    }

    if (m_startTime + _maxLifeTime < Time.fixedTime) {
      _OnMissileBoom();
      return;
    }
  }

  private void _OnMissileBoom(Action cb = null) {
    cb?.Invoke();
    m_active = false;
    vel = Vector2.zero;
    m_animator.SetTrigger("boom");
    m_onMissileBoom?.Invoke(this);
    if (_boomAudio != null) {
      _boomAudio.Play();
    }
  }

  private void _OnRecycle() {
    gameObject.SetActive(false);
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (!m_active || collision.CompareTag("IgnoreMissile")) {
      return;
    }

    if (!collision.CompareTag(m_launcher.tag)) {
      var target = collision.GetComponent<Unit>();
      if (target == null) {
        if (!_penetrable) {
          _OnMissileBoom();
        }
      } else {
        if (target != m_launcher && target.isAlive) {
          _OnMissileBoom(() => target.DoHurt(m_launcher.atk * atkScale));
        }
      }
    }
  }
}