using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfestedTerran : Unit {
  [SerializeField]
  private float _boomDelayTime = 10.0f;
  [SerializeField]
  private GameObject _bomb;
  [SerializeField]
  private Text _timeCounter;

  protected override void Start() {
    base.Start();

    StartCoroutine(_DelayBoom());
  }

  protected override void FixedUpdate() {
    if (!isAlive) {
      return;
    }

    /*
    //移动
    tempVel = Vector2.zero;
    if (Mathf.Abs(vel.x) < MathUtil.epsilon) {
      spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    tempVel.x += direction * attribute.moveSpeed;
    tempVel.y = vel.y;

    //应用速度
    vel = tempVel + attribute.extraVel;
    */
  }

  private IEnumerator _DelayBoom() {
    float startTime = Time.fixedTime;
    while (Time.fixedTime - startTime < _boomDelayTime) {
      float leftTime = _boomDelayTime - (Time.fixedTime - startTime);
      string format = leftTime < 1.0f ? "0.0" : "0";
      _timeCounter.text = leftTime.ToString(format);
      yield return null;
    }
    if (isAlive) {
      var bombGo = Instantiate(_bomb);
      var bomb = bombGo.GetComponent<Bomb>();
      bomb.OnBoom(this, 1.0f, transform.position, () => {
        Destroy(bombGo);
        Destroy(gameObject);
      });
      gameObject.SetActive(false);
    }
  }
}