using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon {
  [SerializeField]
  private SpriteRenderer _raySpriteRenderer;
  private bool m_active;
  private HashSet<Unit> m_damaged = new HashSet<Unit>();

  public override void OnAttack() {
    if (timer.ready && !owner.isSwitchingWeapon) {
      m_active = true;
      _raySpriteRenderer.transform.localScale = new Vector3(-owner.direction, 0.1f, 1.0f);
      _raySpriteRenderer.DOFade(1.0f, 0.2f).OnComplete(() => {
        _raySpriteRenderer.DOFade(0, 0.2f).SetDelay(1.0f).OnComplete(()=> {
          m_active = false;
          m_damaged.Clear();
        });
      });

      timer.OnStart(Time.fixedTime, _attackDelta);
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