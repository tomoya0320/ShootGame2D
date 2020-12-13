using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HpView : MonoBehaviour {
  [SerializeField]
  private Unit _owner;
  [SerializeField]
  private float _updateSpeed;
  private float m_curHpRatio = 1.0f;
  private Image m_hpImage;

  private void Awake() {
    m_hpImage = GetComponent<Image>();
  }

  private void Update() {
    m_curHpRatio = Mathf.Lerp(m_curHpRatio, _owner.hpRatio, _updateSpeed);
    m_hpImage.fillAmount = m_curHpRatio;
    m_hpImage.color = Color.HSVToRGB(m_curHpRatio / 3.0f, 1.0f, 1.0f);
    if(!_owner.isAlive) {
      transform.parent.gameObject.SetActive(false);
    }
  }
}