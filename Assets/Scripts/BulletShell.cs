using System.Collections;
using UnityEngine;

public class BulletShell : MonoBehaviour {
  [SerializeField]
  private float _maxLifeTime;

  private void OnEnable() {
    StartCoroutine(_OnRecycle());
  }

  private IEnumerator _OnRecycle() {
    yield return new WaitForSeconds(_maxLifeTime);
    gameObject.SetActive(false);
  }
}