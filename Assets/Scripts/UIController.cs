using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
  [SerializeField]
  private Sprite _pauseImg;
  [SerializeField]
  private Sprite _resumeImg;

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }
  }

  public void OnPauseOrResume(Image image) {
    Time.timeScale = Time.timeScale == 0 ? 1.0f : 0;
    image.sprite = Time.timeScale == 0 ? _resumeImg : _pauseImg;
  }

  public void OnRestart() {
    Time.timeScale = 1.0f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}