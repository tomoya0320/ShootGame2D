using System.Collections.Generic;
using UnityEngine;

public class PoolManager {
  private List<GameObject> m_pool;
  private Transform m_folder;
  private GameObject m_goPrefab;

  public PoolManager(Transform folder, GameObject goPrefab, int initCapacity = 4) {
    m_folder = folder;
    m_goPrefab = goPrefab;
    m_pool = new List<GameObject>(initCapacity);
    for (int i = 0; i < initCapacity; i++) {
      m_pool.Add(_CreatGameObject());
    }
  }

  public T GetComponent<T>(bool active = true) where T : Component {
    var obj = GetGameObject(active);
    var component = obj.GetComponent<T>();
    if (component == null) {
      component = obj.AddComponent<T>();
    }
    return component;
  }

  public GameObject GetGameObject(bool active = true) {
    foreach (var obj in m_pool) {
      if (!obj.activeSelf) {
        obj.SetActive(active);
        return obj;
      }
    }

    return _CreatGameObject(true);
  }

  private GameObject _CreatGameObject(bool active = false) {
    var obj = Object.Instantiate(m_goPrefab, m_folder);
    obj.SetActive(active);
    m_pool.Add(obj);
    return obj;
  }
}