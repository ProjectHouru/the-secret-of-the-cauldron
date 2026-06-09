using UnityEngine;

public class EnemyDealDamage : BaseCollisionDealDamage
{
    [SerializeField] private float _intervalBetweenDamage = 0.5f;
    [SerializeField] private float _pushForceToPlayer;
    [SerializeField] private int _damageToPlayer;
    
    private float _previousDamageTime;
    
    /// <summary>
    /// Наносим урон игроку
    /// </summary>
    /// <param name="player"></param>
    public override void TakeToPlayer(PlayerTakeDamage player)
    {
        if (_previousDamageTime + _intervalBetweenDamage > Time.time) return;
        for (int i = 0; i < _collision.contactCount; i++)
        {
            var normal = _collision.GetContact(i).normal;
            var dotProduct = Vector2.Dot(normal, Vector2.left);

            if (Mathf.Abs(dotProduct) >= 0.5f)
            {
                _previousDamageTime = Time.time;

                player.HealthPoint.Decrement(_damageToPlayer);

                var direction = Mathf.Round(dotProduct);
                var forceVelocity = new Vector2(_pushForceToPlayer * direction * 2, _pushForceToPlayer);
                _collision.rigidbody.velocity += forceVelocity;

                break;
            }
        }
    }

    /// <summary>
    /// Не наносим урон другим врагам
    /// </summary>
    /// <param name="enemyTakeDamage"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void TakeToEnemy(EnemyTakeDamage enemyTakeDamage)
    {
    }

    public override void TakeToObject(ObjectTakeDamage objectTakeDamage)
    {
       
    }
}