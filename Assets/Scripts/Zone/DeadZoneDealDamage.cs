using UnityEngine;

public class DeadZoneDealDamage : BaseColliderDealDamage
{
    [SerializeField] private float _intervalBetweenDamage = 0.5f;
    [SerializeField] private int _damageToPlayer;
    
    private float _previousDamageTime;

    private void OnTriggerStay2D(Collider2D collider)
    {
        _collider = collider;
        EvaluateCollision(collider.gameObject);
    }
    
    public override void TakeToPlayer(PlayerTakeDamage player)
    {
        if (_previousDamageTime + _intervalBetweenDamage > Time.time) return;

        _previousDamageTime = Time.time;
        player.HealthPoint.Decrement(_damageToPlayer);        
    }

    public override void TakeToEnemy(EnemyTakeDamage enemyTakeDamage)
    {
        if (_previousDamageTime + _intervalBetweenDamage > Time.time) return;

        _previousDamageTime = Time.time;
        enemyTakeDamage.HealthPoint.Decrement(_damageToPlayer);
    }

    public override void TakeToObject(ObjectTakeDamage objectTakeDamage)
    {
        
    }
}