using UnityEngine;

public class GunWithBomb : Gun {
  [SerializeField]
  protected GameObject _bombPrefab;
  [SerializeField]
  protected int _boomDelta = 1;
  protected PoolManager bombPool;

  public override void OnInit(Unit owner) {
    base.OnInit(owner);
    bombPool = new PoolManager(poolFolder, _bombPrefab);
    onMissileBoom = missile => {
      if (shotCount % _boomDelta == 0) {
        var bomb = bombPool.GetComponent<Bomb>();
        bomb.OnBoom(owner, missile.atkScale, missile.transform.position);
      }
    };
  }
}