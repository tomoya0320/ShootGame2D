using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JumpManager : MonoBehaviour {
  [SerializeField]
  public int maxJumpNum = 1;
  private int m_curJumpNum;
  public bool isGrounded => m_curJumpNum == 0;
  private AudioSource m_audioSource;
  public Unit unit { private set; get; }

  private void Awake() {
    m_audioSource = GetComponent<AudioSource>();
  }

  public void OnInit(Unit unit) {
    this.unit = unit;
    m_curJumpNum = 0;
  }

  public bool TryJump() {
    if (m_curJumpNum < maxJumpNum) {
      m_curJumpNum++;
      m_audioSource.Play();
      return true;
    }
    return false;
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Platform") && unit.vel.y < MathUtil.epsilon) {
      m_curJumpNum = 0;
    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.CompareTag("Platform") && unit.vel.y < MathUtil.epsilon) {
      m_curJumpNum = Mathf.Clamp(m_curJumpNum + 1, 0, maxJumpNum);
    }
  }
}