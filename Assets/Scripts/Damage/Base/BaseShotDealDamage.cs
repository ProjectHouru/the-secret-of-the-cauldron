using UnityEngine;

public abstract class BaseShotDealDamage : BaseCollisionDealDamage
{
    [SerializeField] private GameObject _destroyFx;

    public abstract override void TakeToPlayer(PlayerTakeDamage player);

    public abstract override void TakeToEnemy(EnemyTakeDamage enemyTakeDamage);

    public abstract override void TakeToObject(ObjectTakeDamage objectTakeDamage);

    protected override void ToUndamaged()
    {
        SelfDestroy();
    }

    protected void SelfDestroy()
    {
        Instantiate(_destroyFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}