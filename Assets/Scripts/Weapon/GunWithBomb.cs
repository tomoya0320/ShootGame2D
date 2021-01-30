using UnityEngine;

public class GunWithBomb : Gun {
  [SerializeField]
  protected GameObject bombPrefab;
  [SerializeField]
  protected int boomDelta = 1;
  protected PoolManager bombPool;

  public override void OnInit(Unit owner) {
    base.OnInit(owner);
    bombPool = new PoolManager(poolFolder, bombPrefab);
    onMissileBoom = missile => {
      if (shotCount % boomDelta == 0) {
        var bomb = bombPool.GetComponent<Bomb>();
        bomb.OnBoom(owner, missile.atkScale, missile.transform.position);
      }
    };
  }
}