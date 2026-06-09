using UnityEngine;

public abstract class BaseColliderDealDamage : MonoBehaviour, IDealDamage
{
    protected Collider2D _collider;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _collider = collider;
        EvaluateCollision(collider.gameObject);
    }

    protected void EvaluateCollision(GameObject go)
    {
        if (go.TryGetComponent<ITakeDamage>(out var takeDamage))
        {
            takeDamage.Accept(this);
        }
        else
        {
            ToUndamaged();
        }
    }

    public abstract void TakeToPlayer(PlayerTakeDamage player);

    public abstract void TakeToEnemy(EnemyTakeDamage enemyTakeDamage);

    public abstract void TakeToObject(ObjectTakeDamage objectTakeDamage);

    protected virtual void ToUndamaged() { }
}