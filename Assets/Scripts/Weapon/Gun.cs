using System.Collections;
using UnityEngine;

public class Gun : Weapon {
  [SerializeField]
  protected Transform shootMuzzle;
  [SerializeField]
  protected GameObject bulletShellPrefab;
  [SerializeField]
  protected Transform bulletShellMuzzle;
  [SerializeField]
  protected GameObject missilePrefab;
  protected Transform poolFolder;
  [SerializeField]
  protected int onShotNum = 1;
  [SerializeField]
  protected int shotAngleDelta;
  [SerializeField]
  protected int randomAngle;
  [SerializeField]
  protected int _initAngle;
  [SerializeField]
  protected float recoilVel;
  [SerializeField]
  protected float recoilTime;
  protected bool recoiling;
  protected int shotCount;
  protected PoolManager missilePool;
  protected PoolManager bulletShellPool;
  protected System.Action<Missile> onMissileBoom;

  public override void OnInit(Unit owner) {
    base.OnInit(owner);
    //TODO:优化
    poolFolder = GameObject.Find("Pool")?.transform;
    missilePool = new PoolManager(poolFolder, missilePrefab);
    bulletShellPool = new PoolManager(poolFolder, bulletShellPrefab);
  }

  public override void OnAttack() {
    if (timer.ready && !owner.isSwitchingWeapon) {
      shotCount++;

      timer.OnStart(Time.fixedTime, _attackDelta);

      float initAngle = -0.5f * (onShotNum - 1) * shotAngleDelta + _initAngle;
      for (int i = 0; i < onShotNum; i++) {
        var missile = missilePool.GetComponent<Missile>();
        Vector3 initPos = shootMuzzle.position;
        initPos.x = owner.position.x + owner.direction *
                   (owner.position.x - shootMuzzle.position.x);
        float angle = Random.Range(-randomAngle, randomAngle) + initAngle + i * shotAngleDelta;
        missile.OnLaunch(owner, _atkScale, angle, initPos, onMissileBoom);

        initPos = bulletShellMuzzle.position;
        initPos.x = owner.position.x + owner.direction *
                   (owner.position.x - bulletShellMuzzle.position.x);
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

  protected virtual IEnumerator _DoRecoil(int direction) {
    owner.AppendExtralVel(direction * recoilVel * Vector2.left);
    yield return new WaitForSeconds(recoilTime);
    owner.AppendExtralVel(direction * recoilVel * Vector2.right);
    recoiling = false;
  }
}