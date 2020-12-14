using UnityEngine;

public class Gun : Weapon {
  [SerializeField]
  protected Transform _shootMuzzle;
  [SerializeField]
  private GameObject _bulletShellPrefab;
  [SerializeField]
  private Transform _bulletShellMuzzle;
  [SerializeField]
  protected GameObject _missilePrefab;
  protected Transform poolFolder;
  [SerializeField]
  protected int _onShotNum = 1;
  [SerializeField]
  protected int _shotAngleDelta;
  [SerializeField]
  protected int _randomAngle;
  protected int shotCount;
  protected PoolManager missilePool;
  protected PoolManager bulletShellPool;
  protected System.Action<Missile> onMissileBoom;

  public override void OnInit(Unit owner) {
    base.OnInit(owner);
    //TODO:优化
    poolFolder = GameObject.Find("Pool")?.transform;
    missilePool = new PoolManager(poolFolder, _missilePrefab);
    bulletShellPool = new PoolManager(poolFolder, _bulletShellPrefab);
  }

  public override void OnAttack() {
    if (timer.ready && !owner.isSwitchingWeapon) {
      shotCount++;

      timer.OnStart(Time.fixedTime, _attackDelta);

      float initAngle = -0.5f * (_onShotNum - 1) * _shotAngleDelta;
      for (int i = 0; i < _onShotNum; i++) {
        var missile = missilePool.GetComponent<Missile>();
        Vector3 initPos = _shootMuzzle.position;
        initPos.x = owner.position.x + owner.direction *
                   (owner.position.x - _shootMuzzle.position.x);
        float angle = Random.Range(-_randomAngle, _randomAngle) + initAngle + i * _shotAngleDelta;
        missile.OnLaunch(owner, _atkScale, angle, initPos, onMissileBoom);

        initPos = _bulletShellMuzzle.position;
        initPos.x = owner.position.x + owner.direction *
                   (owner.position.x - _bulletShellMuzzle.position.x);
        Vector2 bulletShellDir = Vector2.one;
        bulletShellDir.x *= -owner.direction;
        var obj = bulletShellPool.GetGameObject();
        obj.transform.position = initPos;
        obj.GetComponent<Rigidbody2D>()?.AddForce(bulletShellDir * 250.0f);
      }

      if (owner is Character) {
        CameraController.DoShake();
      }
      if (!recoiling) {
        recoiling = true;
        StartCoroutine(_DoRecoil(owner.direction));
      }

      if (_attackAudio != null) {
        _attackAudio.Play();
      }
    }
  }
}