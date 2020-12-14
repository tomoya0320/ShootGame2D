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
  protected JumpManager jumpManager;
  [SerializeField]
  protected Attribute attribute;
  [SerializeField]
  protected List<Weapon> weapons;
  protected int curWeaponIdx;
  protected Weapon currentWeapon;
  protected Animator animator;
  private Rigidbody2D m_rigidbody2D;
  protected SpriteRenderer spriteRenderer;
  //经常用到的临时变量
  protected Vector2 tempVel;
  protected AnimTriggerState triggerState = AnimTriggerState.NONE;
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

  protected virtual void Awake() {
    animator = GetComponent<Animator>();
    m_rigidbody2D = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();

    foreach (var weapon in weapons) {
      weapon.OnInit(this);
    }
    curWeaponIdx = weapons.Count > 0 ? 0 : -1;
    currentWeapon = curWeaponIdx >= 0 ? weapons[curWeaponIdx] : null;
    jumpManager.OnInit(this);

    //临时用
    m_maxHp = attribute.hp;
  }

  protected virtual void Start() {

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

  protected void OnSwitchWeapon() {
    if (curWeaponIdx >= 0) {
      curWeaponIdx = (curWeaponIdx + 1) % weapons.Count;
    }
    currentWeapon = weapons[curWeaponIdx];
  }

  protected virtual void OnJump() {
    if (jumpManager.TryJump()) {
      tempVel = vel;
      tempVel.y = attribute.jumpSpeed;
      vel = tempVel;
    }
  }

  public void AppendExtralVel(Vector2 vel) {
    attribute.extraVel += vel;
  }

  public void Bound(float boundSpeed) {
    vel = new Vector2(vel.x, boundSpeed);
  }

  public void DoHurt(float damage) {
    if(damage >= attribute.hp) {
      attribute.hp = 0;
      vel = Vector2.zero;
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

  }
}