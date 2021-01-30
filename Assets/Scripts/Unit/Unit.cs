using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Attribute {
  public float hp;
  public float atk;
  public float jumpSpeed;
  public float moveSpeed;
  [NonSerialized]
  public Vector2 extraVel;
}

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public abstract class Unit : MonoBehaviour {
  /// <summary>
  /// 同一帧里触发两个trigger动画会出问题
  /// </summary>
  protected enum AnimTriggerState {
    NONE, HURT, DEAD
  }

  //临时用
  private float m_maxHp;
  [SerializeField]
  protected Attribute attribute;
  [SerializeField]
  protected List<Weapon> weapons;
  [SerializeField]
  protected JumpManager jumpManager;
  [SerializeField]
  protected AudioSource switchWeaponAudio;
  protected int curWeaponIdx;
  public Weapon currentWeapon {
    get {
      if (curWeaponIdx >= 0 && curWeaponIdx < weapons.Count) {
        return weapons[curWeaponIdx];
      }
      return null;
    }
  }
  protected Animator animator;
  private Rigidbody2D m_rigidbody2D;
  protected SpriteRenderer spriteRenderer;
  //经常用到的临时变量
  protected Vector2 tempVel;
  protected AnimTriggerState triggerState = AnimTriggerState.NONE;
  public bool isSwitchingWeapon => switchWeaponAudio != null && switchWeaponAudio.isPlaying;
  public int direction => spriteRenderer.flipX ? 1 : -1;
  public bool flipX => spriteRenderer.flipX;
  public Vector3 position => transform.position;
  public bool isAlive => attribute.hp > 0;
  public float atk => attribute.atk;
  public float hpRatio => attribute.hp / m_maxHp;
  public Vector3 vel {
    get {
      return m_rigidbody2D.velocity;
    }
    protected set {
      m_rigidbody2D.velocity = value;
    }
  }
  public float gravity {
    get {
      return m_rigidbody2D.gravityScale;
    }
    protected set {
      m_rigidbody2D.gravityScale = value;
    }
  }
  protected bool simulated {
    get {
      return m_rigidbody2D.simulated;
    }
    set {
      m_rigidbody2D.simulated = value;
    }
  }

  protected virtual void Awake() {
    animator = GetComponent<Animator>();
    m_rigidbody2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  protected virtual void Start() {
    foreach (var weapon in weapons) {
      weapon.OnInit(this);
    }

    curWeaponIdx = weapons.Count > 0 ? 0 : -1;
    jumpManager?.OnInit(this);

    //临时用
    m_maxHp = attribute.hp;
  }

  protected virtual void Update() {
    UpdateAnimState();
  }

  protected virtual void FixedUpdate() { }

  protected virtual void UpdateAnimState() {
    //一帧只触发一个anystate的trigger
    if (triggerState != AnimTriggerState.NONE) {
      switch (triggerState) {
        case AnimTriggerState.HURT: {
            animator.SetTrigger("hurt");
            break;
          }
        case AnimTriggerState.DEAD: {
            animator.SetTrigger("dead");
            break;
          }
      }
      triggerState = AnimTriggerState.NONE;
    }
  }

  protected virtual void OnSwitchWeapon() {
    if (isSwitchingWeapon) {
      return;
    }
    currentWeapon?.OnSwitchOut();
    curWeaponIdx = (curWeaponIdx + 1) % weapons.Count;
    currentWeapon?.OnSwitchIn();
    if (switchWeaponAudio != null) {
      switchWeaponAudio.Play();
    }
  }

  protected virtual void OnJump() {
    if (jumpManager?.TryJump() ?? false) {
      tempVel = vel;
      tempVel.y = attribute.jumpSpeed;
      vel = tempVel;
    }
  }

  public void AppendExtralVel(Vector2 vel) {
    attribute.extraVel += vel;
  }

  public void Bound(Vector2 boundVel) {
    vel = boundVel;
  }

  public void DoHurt(float damage) {
    if (damage >= attribute.hp) {
      attribute.hp = 0;
    } else {
      attribute.hp -= damage;
    }

    if (isAlive) {
      triggerState = AnimTriggerState.HURT;
    } else {
      triggerState = AnimTriggerState.DEAD;
      OnDie();
    }
  }

  public void DoRecover(float hp) {
    attribute.hp = Mathf.Min(attribute.hp + hp, m_maxHp);
  }

  protected virtual void OnDie() {
    tempVel = Vector2.one * 250.0f;
    tempVel.x *= -direction;
    vel = Vector2.zero;
    m_rigidbody2D.sharedMaterial = null;
    m_rigidbody2D.AddForce(tempVel);
  }
}