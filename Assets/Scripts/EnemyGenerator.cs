using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
  [Serializable]
  private struct GeneratorData {
    public int num;
    public float deltaTime;
    public float delayTime;
    public GameObject prefab;
    public Transform bornPoint;
  }

  [SerializeField]
  private Transform _folder;
  [SerializeField]
  private List<GeneratorData> _waves;

  private IEnumerator Start() {
    foreach (var data in _waves) {
      yield return new WaitForSeconds(data.delayTime);
      for (int i = 0; i < data.num; i++) {
        Instantiate(data.prefab, data.bornPoint.position, Quaternion.identity, _folder);
        yield return new WaitForSeconds(data.deltaTime);
      }
    }
  }
}