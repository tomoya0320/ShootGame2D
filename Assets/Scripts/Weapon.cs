using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
  [SerializeField]
  protected float _attackDelta = 0.1f;
  [SerializeField]
  protected float _atkScale = 1.0f;
  public float atkScale => _atkScale;
  [SerializeField]
  protected float _recoilVel; 
  [SerializeField]
  protected float _recoilTime;
  protected bool recoiling;
  protected Timer timer;
  protected Unit owner;

  public virtual void OnInit(Unit owner) {
    this.owner = owner;
    timer = new Timer();
  }

  protected virtual void FixedUpdate() {
    timer.OnUpdate(Time.fixedDeltaTime);
  }

  public abstract void OnAttack();

  protected virtual IEnumerator _DoRecoil(int direction) {
    owner.AppendExtralVel(direction * _recoilVel * Vector2.left);
    yield return new WaitForSeconds(_recoilTime);
    owner.AppendExtralVel(direction * _recoilVel * Vector2.right);
    recoiling = false;
  }
}