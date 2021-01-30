using UnityEngine;

public class OperationButton : MonoBehaviour {
  [SerializeField]
  private PressKey _pressKey;
  public PressKey pressKey => _pressKey;
}