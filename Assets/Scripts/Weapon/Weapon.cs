using UnityEngine;

public abstract class Weapon : MonoBehaviour {
  [SerializeField]
  protected AudioSource _attackAudio;
  [SerializeField]
  protected float _attackDelta = 0.1f;
  [SerializeField]
  protected float _atkScale = 1.0f;
  protected Unit owner;
  protected Timer timer = new Timer();

  public virtual void OnInit(Unit owner) {
    this.owner = owner;
  }

  protected virtual void FixedUpdate() {
    timer.OnUpdate(Time.fixedDeltaTime);
  }

  public abstract void OnAttack();

  public virtual void OnSwitchIn() { }

  public virtual void OnSwitchOut() { }
}