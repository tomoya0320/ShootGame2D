using UnityEngine;

public class CameraTrigger : MonoBehaviour {
  [SerializeField]
  private float _size; 
  [SerializeField]
  private Vector2 _offset;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player")) {
      CameraController.AppendCameraValue(_offset, _size);
      _size *= -1.0f;
      _offset *= -1.0f;
    }
  }
}