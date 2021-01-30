using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon {
  [SerializeField]
  private SpriteRenderer _raySpriteRenderer;
  [SerializeField]
  private SpriteRenderer _muzzleSpriteRenderer;
  [SerializeField]
  private Vector2 _muzzleMoveRange;
  private bool m_active;
  private HashSet<Unit> m_damaged = new HashSet<Unit>();
  private float m_moveTime = 1.0f;
  private float m_warningTime = 2.0f;
  private float m_rayFadeTime = 0.2f;
  private float m_rayDuration = 0.6f;

  public override void OnAttack() {
    if (timer.ready && !owner.isSwitchingWeapon) {
      float y = Random.Range(_muzzleMoveRange.x, _muzzleMoveRange.y);
      _muzzleSpriteRenderer.transform.DOLocalMoveY(y, m_moveTime).OnComplete(() => {
        _muzzleSpriteRenderer.DOColor(Color.red, m_warningTime).OnComplete(() => {
          m_active = true;
          _raySpriteRenderer.DOFade(1.0f, m_rayFadeTime).OnComplete(() => {
            _raySpriteRenderer.DOFade(0, m_rayFadeTime).SetDelay(m_rayDuration).OnComplete(() => {
              m_active = false;
              m_damaged.Clear();
              _muzzleSpriteRenderer.DOColor(Color.white, m_warningTime).OnComplete(()=> {
              });
            });
          });
        });
      });
      timer.OnStart(Time.fixedTime, _attackDelta + m_moveTime +
        m_rayDuration + m_warningTime * 2.0f + m_rayFadeTime * 2.0f);
    }
  }

  private void OnTriggerStay2D(Collider2D collision) {
    if (!m_active) {
      return;
    }

    if (!collision.CompareTag(owner.tag)) {
      var target = collision.GetComponent<Unit>();
      if (target != null && !m_damaged.Contains(target) && target != owner && target.isAlive) {
        target.DoHurt(owner.atk * _atkScale);
        m_damaged.Add(target);
      }
    }
  }
}