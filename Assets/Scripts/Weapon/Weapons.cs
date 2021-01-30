using System.Collections.Generic;
using UnityEngine;

public class Weapons : Weapon {
  [SerializeField]
  private List<Weapon> _weapons;

  public override void OnInit(Unit owner) {
    base.OnInit(owner);

    foreach (var weapon in _weapons) {
      weapon.OnInit(owner);
      weapon.AddValue(_attackDelta, _atkScale);
    }
  }

  public override void OnAttack() {
    foreach (var weapon in _weapons) {
      weapon.OnAttack();
    }
  }
}