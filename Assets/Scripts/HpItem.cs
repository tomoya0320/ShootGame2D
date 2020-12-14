using System.Collections.Generic;
using UnityEngine;

public class HpItem : MonoBehaviour {
  [SerializeField]
  private float _hp;
  [SerializeField]
  private List<string> _filterTags;

  private void OnTriggerEnter2D(Collider2D collision) {
    var unit = collision.GetComponent<Unit>();
    if (unit != null && _filterTags.Contains(collision.tag)) {
        unit.DoRecover(_hp);
        Destroy(gameObject);
    }
  }
}