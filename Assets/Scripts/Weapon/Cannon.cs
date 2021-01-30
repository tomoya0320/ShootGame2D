using UnityEngine;

public class Cannon : Weapon {
  [SerializeField]
  protected Transform shootMuzzle;
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
  protected int shotCount;
  protected PoolManager missilePool;
  protected System.Action<Missile> onMissileBoom;

  public override void OnInit(Unit owner) {
    base.OnInit(owner);
    //TODO:”≈ªØ
    poolFolder = GameObject.Find("Pool")?.transform;
    missilePool = new PoolManager(poolFolder, missilePrefab);
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
      }

      if (owner is Character) {
        CameraController.DoShake();
      }

      if (_attackAudio != null) {
        _attackAudio.Play();
      }
    }
  }
}