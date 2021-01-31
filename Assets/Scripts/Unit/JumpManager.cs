using UnityEngine;

public class JumpManager : MonoBehaviour {
  [SerializeField]
  private int _maxJumpNum = 1;
  [SerializeField]
  private AudioSource _jumpAudio;
  private int m_curJumpNum;
  public bool isGrounded => m_curJumpNum == 0;
  public Unit unit { private set; get; }

  public void OnInit(Unit unit) {
    this.unit = unit;
  }

  public bool TryJump() {
    if (m_curJumpNum < _maxJumpNum) {
      m_curJumpNum++;
      if (_jumpAudio != null) {
        _jumpAudio.Play();
      }
      return true;
    }
    return false;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Platform") /*&& unit.vel.y < MathUtil.epsilon*/) {
      m_curJumpNum = 0;
    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.CompareTag("Platform") && unit.vel.y < MathUtil.epsilon) {
      m_curJumpNum = Mathf.Clamp(m_curJumpNum + 1, 0, _maxJumpNum);
    }
  }
}