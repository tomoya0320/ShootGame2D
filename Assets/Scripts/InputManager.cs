using System;
using System.Collections.Generic;
using UnityEngine;

public enum PressKey {
  LEFT, RIGHT, JUMP, ATTACK, SWITCH_WEAPON
}

public class InputManager : MonoBehaviour {
  [Serializable]
  private struct KeyMap {
    public string name;
    public PressKey pressKey;
  }

  [SerializeField]
  private KeyMap[] _keyMaps;
  private Dictionary<PressKey, bool> m_pressKeyDict = new Dictionary<PressKey, bool>();
  private Dictionary<PressKey, Action> m_upEventDict = new Dictionary<PressKey, Action>();
  private Dictionary<PressKey, Action> m_downEventDict = new Dictionary<PressKey, Action>();
  public static InputManager instance;

  private void Awake() {
    instance = this;

    foreach (var keyMap in _keyMaps) {
      m_pressKeyDict.Add(keyMap.pressKey, false);
    }
  }

  private void Update() {
    if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer) {
      foreach (var keyMap in _keyMaps) {
        m_pressKeyDict[keyMap.pressKey] = Input.GetButton(keyMap.name);
        if (m_downEventDict.ContainsKey(keyMap.pressKey) && Input.GetButtonDown(keyMap.name)) {
          m_downEventDict[keyMap.pressKey]?.Invoke();
        }
      }
    }
  }

  public void SetButtonDown(OperationButton button) {
    m_pressKeyDict[button.pressKey] = true;
    if (m_downEventDict.ContainsKey(button.pressKey)) {
      m_downEventDict[button.pressKey]?.Invoke();
    }
  }

  public void SetButtonUp(OperationButton button) {
    m_pressKeyDict[button.pressKey] = false;
    if (m_upEventDict.ContainsKey(button.pressKey)) {
      m_upEventDict[button.pressKey]?.Invoke();
    }
  }

  public static bool GetButton(PressKey pressKey) {
    if (!instance.m_pressKeyDict.ContainsKey(pressKey)) {
      return false;
    }
    return instance.m_pressKeyDict[pressKey];
  }

  public static void RegisterDownEvent(PressKey pressKey, Action cb) {
    if (instance.m_downEventDict.ContainsKey(pressKey)) {
      instance.m_downEventDict[pressKey] += cb;
    } else {
      instance.m_downEventDict.Add(pressKey, cb);
    }
  }

  public static void RemoveDownEvent(PressKey pressKey, Action cb) {
    if (instance.m_downEventDict.ContainsKey(pressKey)) {
      if (instance.m_downEventDict[pressKey] != null) {
        instance.m_downEventDict[pressKey] -= cb;
      }
    }
  }

  public static void RegisterUpEvent(PressKey pressKey, Action cb) {
    if (instance.m_upEventDict.ContainsKey(pressKey)) {
      instance.m_upEventDict[pressKey] += cb;
    } else {
      instance.m_upEventDict.Add(pressKey, cb);
    }
  }

  public static void RemoveUpEvent(PressKey pressKey, Action cb) {
    if (instance.m_upEventDict.ContainsKey(pressKey)) {
      if (instance.m_upEventDict[pressKey] != null) {
        instance.m_upEventDict[pressKey] -= cb;
      }
    }
  }
}