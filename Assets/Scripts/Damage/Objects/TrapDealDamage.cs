using UnityEngine;

public class TrapDealDamage : BaseCollisionDealDamage
{
    [SerializeField] private float _intervalBetweenDamage = 0.5f;
    [SerializeField] private float _pushForceToPlayer;
    [SerializeField] private int _damageToPlayer;
    private float _previousDamageTime;

    public override void TakeToPlayer(PlayerTakeDamage player)
    {
        if (_previousDamageTime + _intervalBetweenDamage > Time.time) return;

        _previousDamageTime = Time.time;
        player.HealthPoint.Decrement(_damageToPlayer);
        var forceVelocity = Vector2.up * _pushForceToPlayer;
        _collision.rigidbody.velocity += forceVelocity;            
    }

    public override void TakeToEnemy(EnemyTakeDamage enemyTakeDamage)
    {
        if (_previousDamageTime + _intervalBetweenDamage > Time.time) return;

        _previousDamageTime = Time.time;
        enemyTakeDamage.HealthPoint.Decrement(_damageToPlayer);
        var forceVelocity = Vector2.up * _pushForceToPlayer;
        _collision.rigidbody.velocity += forceVelocity;  
    }

    public override void TakeToObject(ObjectTakeDamage objectTakeDamage)
    {
        
    }
}