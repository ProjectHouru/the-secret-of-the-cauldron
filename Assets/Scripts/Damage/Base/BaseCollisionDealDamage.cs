using UnityEngine;

public abstract class BaseCollisionDealDamage : MonoBehaviour, IDealDamage
{
    protected Collision2D _collision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collision = collision;
        EvaluateCollision(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _collision = collision;
        EvaluateCollision(collision.gameObject);
    }

    private void EvaluateCollision(GameObject go)
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